namespace YnabSharpa.Collections;

public record TransactionsByMemoOccurrenceByPayeeName(
    string PayeeName, 
    IEnumerable<TransactionsByMemoOccurrence> TransactionsByMemoOccurrences);