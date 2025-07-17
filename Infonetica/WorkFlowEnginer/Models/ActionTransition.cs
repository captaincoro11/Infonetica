// Action Transition Model
namespace WorkFlowEngine.Models;

public record ActionTransition(
    string Id,
    bool Enabled,
    List<string> FromStates,
    string ToState
);
