// WorkFlowInstance Model
namespace WorkFlowEngine.Models;

public record ActionHistory(
    string ActionId,
    DateTime Timestamp
);

public record WorkFlowInstance(
    string Id,
    string DefinitionId,
    string currentState,
    List<ActionHistory> History
);
