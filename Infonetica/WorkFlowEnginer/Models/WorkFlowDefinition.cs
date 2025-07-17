// WorkFlow Defintion Model
namespace WorkFlowEngine.Models;

public record WorkFlowDefinition(
        string Id,
        List<State> States,
        List<ActionTransition> Actions
);