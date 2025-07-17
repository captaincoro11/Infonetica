// State Model
namespace WorkFlowEngine.Models;

public record State(
    string Id,
    bool isInitial,
    bool isFinal,
    bool Enabled,
    string Description
);