# Automated & transfer transactions

## Premise

Not every transaction YNAB returns represents real spending. Two kinds
don't: **transfers**, which move money between two of a plan's own
accounts, and **automated entries**, which YNAB itself creates to keep
an account's balance correct rather than to record a purchase. Code
that totals up spending, or that moves transactions from one account to
another, needs to recognise both.

## Problem

The two kinds are flagged differently on the wire, so a caller can't
check one property and catch both. A transfer carries a
`transfer_transaction_id` linking it to its matching entry on the other
account. An automated entry carries no such link — the only signal is
its payee name, which YNAB assigns from a fixed, small set of strings
("Starting Balance", "Manual Balance Adjustment", "Reconciliation
Balance Adjustment"). Getting "just my real spending" out of a
transaction list means applying both checks, not one.

## Solution

**Transfers** are exposed as `SplitTransactions.IsTransfer`
(`YnabSharp/SplitTransactions.cs`), true when
`TransferTransactionId` is present on the underlying
`SplitTransactionResponse`:

```csharp
public bool IsTransfer => !string.IsNullOrEmpty(splitTransactionResponse.TransferTransactionId);
```

Because `Transaction : SplitTransactions`, this is available on both a
whole transaction and on any of its
[split line items](split-transactions.md).

**Automated entries** are a fixed list, not a computed property:
`AutomatedPayeeNames` (`YnabSharp/AutomatedPayeeNames.cs`) defines the
three known payee-name constants and an `All` list of them:

```csharp
public const string StartingBalance = "Starting Balance";
public const string ManualBalanceAdjustment = "Manual Balance Adjustment";
public const string ReconciliationBalanceAdjustment = "Reconciliation Balance Adjustment";
```

Detecting one means comparing a transaction's `PayeeName` against this
list — there's no `IsAutomated` property on `Transaction` itself.

`TransactionExtensions` (`YnabSharp/Extensions/TransactionExtensions.cs`)
turns both checks into filters over `IEnumerable<Transaction>`:

| Method | Excludes |
|---|---|
| `FilterOutTransfers()` | transactions where `IsTransfer` |
| `FilterOutAutomations()` | transactions whose `PayeeName` is in `AutomatedPayeeNames.All` |
| `FilterToSpending()` | both of the above, combined |

`FilterToSpending()` is the one to reach for when you want "what did I
actually spend," since transfers and automated balance entries both
inflate a raw sum otherwise.

A third, narrower rule lives in
`ConnectedPlan.MoveAccountTransactions` (see
[connected domain objects](connected-domain-objects.md)): when moving a
transaction history from one account to another, it excludes only
`AutomatedPayeeNames.StartingBalance` — not the full `All` list, and not
transfers — before moving the rest:

```csharp
var transactionsToMove = transactionsTask
    .Result
    .Where(t => t.PayeeName != AutomatedPayeeNames.StartingBalance)
    .ToMovedTransactions(toAccount.Id);
```

The reasoning is specific to that one payee: a starting-balance entry
describes the *old* account's opening balance, so it shouldn't follow
the money to a new account. A manual or reconciliation balance
adjustment, and a transfer, don't carry that same meaning — they're
left in the moved set today.

## Constraints & tradeoffs

**Automated-entry detection is payee-name string matching, not a wire
flag.** `AutomatedPayeeNames` hardcodes YNAB's English payee text. If
YNAB ever localises these names, or a real payee a user creates happens
to collide with one of the three strings, `FilterOutAutomations()` and
`FilterToSpending()` would misclassify it. Transfer detection doesn't
have this problem — `IsTransfer` reads an actual API field.

**Transfers and automated entries use two unrelated signals**, with no
shared concept (an `IsSystemGenerated` or similar) tying them together
in the domain model. A caller filtering for "real spending only" has to
know to combine `FilterOutTransfers()` and `FilterOutAutomations()` (or
just call `FilterToSpending()`) rather than checking one flag.

**None of this exists for `ScheduledTransaction`.** It exposes `Id`,
`Amount`, `NextOccurence`, and `AccountId` only — no `IsTransfer`, no
payee name, so there's no way to apply the same filtering to upcoming
scheduled transactions today.

**`MoveAccountTransactions` only excludes `StartingBalance`, not the
full `AutomatedPayeeNames.All` list or transfers.** Whether a manual
balance adjustment or a transfer transaction *should* also be excluded
when moving accounts isn't addressed anywhere in code or docs — this is
a genuine open question, not a documented decision, and isn't tracked
as an issue as of this writing.

## Questions & answers

**How do I get a transaction list with only real spending?**
`transactions.FilterToSpending()` — drops both transfers and automated
balance entries in one call.

**How do I tell a transfer apart from an automated balance entry?**
`transaction.IsTransfer` for transfers;
`AutomatedPayeeNames.All.Contains(transaction.PayeeName)` for automated
entries. They're independent checks — a transaction could in principle
be neither, either, but not typically both.

**Does moving an account's transactions with `MoveAccountTransactions`
leave transfers and balance adjustments behind, like it does starting
balances?**
No — only `PayeeName == AutomatedPayeeNames.StartingBalance` is
filtered out before the move. Transfers and the other two automated
payee types are moved along with everything else. Verify this matches
what you need before relying on it; it may not be intentional.

**Can I filter scheduled transactions for transfers or automations the
same way?**
Not today. `ScheduledTransaction` doesn't expose a payee name or
`IsTransfer`, so `FilterOutTransfers()`/`FilterOutAutomations()` only
work on `IEnumerable<Transaction>`.
