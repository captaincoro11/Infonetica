using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WorkFlowEngine.Models;
using WorkFlowEngine.Services;

namespace WorkFlowEngine.Controllers;

[ApiController]
[Route("[controller]")]

public class WorkFlowController : ControllerBase
{
    private readonly WorkFlowService _workFlowService;

    public WorkFlowController(WorkFlowService workFlowService)
    {
        _workFlowService = workFlowService;
    }

    // Creating Definition using action and states (http://localhost:5232/workflow/definition)
    [HttpPost("definition")]
    public ActionResult CreateDefinition([FromBody] WorkFlowDefinition def)
    {   
        bool isValid = _workFlowService.validateDefinition(def);

        if (!isValid)
        {
            return BadRequest("Invalid definition: Must have one unique initial state");
        }

        _workFlowService.AddDefinition(def);

        return Ok(def);
    }

    // Fetching Defintion Using Definition Id (http://localhost:5232/workflow/definition/{definitionId})
    [HttpGet("definition/{id}")]
    public ActionResult GetDefinition(string id)
    {
        var def = _workFlowService.GetDefinition(id);

        if (def == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(def);
        }
    }

    // Creating An Instance Using definitionId (http://localhost:5232/workflow/instance/{definitionId})
    [HttpPost("instance/{definitionId}")]
    public ActionResult StartInstance(string definitionId)
    {
        var instance = _workFlowService.StartInstance(definitionId);

        if (instance == null)
        {
            return NotFound("Definition Not Found");
        }
        else
        {
            return Ok(instance);
        }
    }

    // Moving From One State To Other (http://localhost:5232/workflow/instance/{instanceId}/action/{actionId})
    [HttpPost("instance/{id}/action/{actionId}")]
    public ActionResult ExecuteAction(string id, string actionId)
    {
        var result = _workFlowService.ExecuteAction(id, actionId);

        if (result.success)
        {
            return Ok(result.instance);
        }

        else
        {
            return BadRequest(result.message);
        }
    }

    // Fetching The Instance Using instanceId (http://localhost:5232/workflow/instance/{instanceId})
    [HttpGet("instance/{id}")]
    public ActionResult GetInstance(string id)
    {
        var instance = _workFlowService.GetInstance(id);

        if (instance == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(instance);
        }
    }

}