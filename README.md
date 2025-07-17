# INFONETICA ASSIGNMENT
## Initial Setup
First Clone This Repository
```
$ git clone https://github.com/captaincoro11/Infonetica.git
```
After Cloning Run These Commands
```
$ cd Infonetica
$ cd WorkFlowEnginer
$ dotnet run
```
After Running Above commands the server will start running on 
```
$ http://localhost:5232/workflow
```

## Approach Used 
Made use of MVC Architecture here separate folders for Models, Controllers and Services to maintain a clean and modular code

## APIs implemented
1-> Creating a Definition 
```
$ http://localhost:5232/workflow/definition
```
Added body to test above functionality
```
{
  "id": "bookingWorkflow",
  "states": [
    { "id": "requested", "isInitial": false, "isFinal": false, "enabled": true , "description":"hello hi there" },
    { "id": "approved", "isInitial": true, "isFinal": false, "enabled": true , "description":"hello hi there" },
    { "id": "done", "isInitial": false, "isFinal": true, "enabled": true , "description":"hello hi there"}
  ],
  "actions": [
    { "id": "approve", "enabled": true, "fromStates": ["requested"], "toState": "approved" },
    { "id": "complete", "enabled": true, "fromStates": ["approved"], "toState": "done" }
  ]
}
```

2-> Fetching Definition
```
$ http://localhost:5232/workflow/definition/{definitionId}
```
If using above example can replace {definitionId} by bookingWorkflow

3-> Creating an instance using definitionId
```
$ http://localhost:5232/workflow/instance/{definitionId}
```
Replace the above {definitionId} by bookingWorkflow if using above example

4-> Moving From One State to Another
```
$ http://localhost:5232/workflow/instance/{instanceId}/action/{actionId}
```
Replace the above {instanceId} with the one which you get as a response from 3rd number API and in place of {actionId} chose the one which suits your case , for eg in this case as we have kept "approved" state as true so we will select actionId = "complete" so we can move from "approved" to "done"

5-> Fetching the instance 
```
$ http://localhost:5232/workflow/instance/{instanceId}
```
