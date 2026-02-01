namespace YnabSharp.Collections;

public record TransactionsByMemoOccurrenceByPayeeName(
    string PayeeName, 
    IEnumerable<TransactionsByMemoOccurrence> TransactionsByMemoOccurrences);