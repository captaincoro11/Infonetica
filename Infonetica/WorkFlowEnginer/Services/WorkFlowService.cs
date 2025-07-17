using WorkFlowEngine.Models;

namespace WorkFlowEngine.Services;

public class WorkFlowService
{
    // In-Memory Storage 
    private readonly Dictionary<string, WorkFlowDefinition> _definitions = new();
    private readonly Dictionary<string, WorkFlowInstance> _instances = new();

    // Check for only one isInitial == true to be present while creating an instance
    public bool validateDefinition(WorkFlowDefinition def)
    {
        int initialStateCount = 0;
        foreach (var state in def.States)
        {
            if (state.isInitial) initialStateCount++;
        }

        if (initialStateCount > 1) return false;

        List<string> ids = [];
        foreach (var state in def.States)
        {
            if (ids.Contains(state.Id)) return false;
            ids.Add(state.Id);
        }

        return true;
    }

    // Update or add definition after validation
    public void AddDefinition(WorkFlowDefinition def)
    {
        if (_definitions.ContainsKey(def.Id))
        {
            _definitions[def.Id] = def;
        }
        else
        {
            _definitions.Add(def.Id, def);
        }

    }

    // Fetching the definition using definitionId
    public WorkFlowDefinition? GetDefinition(string id)
    {
        if (_definitions.ContainsKey(id))
        {
            return _definitions[id];
        }
        else
        {
            return null;
        }
    }

    // Creating an instance
    public WorkFlowInstance? StartInstance(string definitionId)
    {
        if (!_definitions.ContainsKey(definitionId))
        {
            return null;
        }

        WorkFlowDefinition def = _definitions[definitionId];

        State? initial = null;
        foreach (var state in def.States)
        {
            if (state.isInitial)
            {
                initial = state;
                break;
            }
        }

        if (initial == null) return null;

        string instanceId = Guid.NewGuid().ToString();
    
        WorkFlowInstance instance = new WorkFlowInstance(instanceId, definitionId, initial.Id, new List<ActionHistory>());

        _instances[instance.Id] = instance;

        return instance;

    }

    // Executing actions to move from one state to another 
    public (bool success, string message, WorkFlowInstance? instance) ExecuteAction(string instanceId, string actionId)
    {
        if (!_instances.ContainsKey(instanceId))
        {
            return (false, "Instance Not Found", null);
        }

        WorkFlowInstance instance = _instances[instanceId];

        if (!_definitions.ContainsKey(instance.DefinitionId))
        {
            return (false, "Definition not found", null);
        }

        WorkFlowDefinition def = _definitions[instance.DefinitionId];

        ActionTransition? action = null;
        foreach (var a in def.Actions)
        {
            if (a.Id == actionId)
            {
                action = a;
                break;
            }
        }

        if (action == null)
        {
            return (false, "Action not found", null);
        }

        bool isValidTransition = false;
        foreach (var fromState in action.FromStates)
        {
            if (fromState == instance.currentState)
            {
                isValidTransition = true;
                break;
            }
        }

        if (!isValidTransition)
        {
            return (false, "Invalid transition from current state", null);
        }

        var updatedInstance = instance with
        {
            currentState = action.ToState,
            History = new List<ActionHistory>(instance.History)
            {
                new ActionHistory(action.Id, DateTime.UtcNow)
            }
        };

        _instances[updatedInstance.Id] = updatedInstance;

        return (true, "", updatedInstance);

    }
    
    // Fetching instance using instanceId
    public WorkFlowInstance? GetInstance(string id)
    {
        if (_instances.ContainsKey(id))
        {
            return _instances[id];
        }
        else
        {
            return null;
        }
    }


}