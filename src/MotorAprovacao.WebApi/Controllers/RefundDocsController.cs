using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MotorAprovacao.Data.Repositories;
using MotorAprovacao.Models.Entities;
using MotorAprovacao.Models.Enums;
using MotorAprovacao.WebApi.ErrorHandlers;
using MotorAprovacao.WebApi.Filters;
using MotorAprovacao.WebApi.RequestDtos;
using MotorAprovacao.WebApi.ResponseDtos;
using MotorAprovacao.WebApi.Services;
using System.ComponentModel.DataAnnotations;

namespace MotorAprovacao.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RefundDocsController : ControllerBase
    {
        private readonly IRefundDocumentService _service;
        private readonly ILogger<RefundDocsController> _logger;

        public RefundDocsController(IRefundDocumentService service, ILogger<RefundDocsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Finds a specific Refund Document by Id
        /// </summary>
        /// <param name="id">Id of Refund Document to return</param>
        /// <returns>A Refund Document</returns>
        /// <response code="200">Returns the Refund Document</response>
        /// <response code="404">Refund Document not found</response>
        /// <response code="400">Id format is invalid</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RefundDocumentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(GetById)} requested with 'id' = {id}");

            var documentResult = await _service.GetDocumentById(id);

            if (!documentResult.Success)
                return documentResult.ErrorActionResult!;

            var documentResponseDto = new RefundDocumentResponseDto(documentResult.Value!);

            _logger.LogInformation($"{nameof(GetById)} responded with body");

            return Ok(documentResponseDto);
        }
        /// <summary>
        /// Finds Refund Documents by Status
        /// </summary>
        /// <param name="status">Refund Document's Status
        /// <para>Values: 0 - OnApproval, 1 - Approved, 2 - Disapproved</para>
        /// </param>
        /// <param name="orderBy">Selector item to order the list
        /// <para>Values: total (default), date</para>
        /// </param>
        /// <param name="order">Order to be showed
        /// <para>Values: asc (default), desc</para>
        /// </param>
        /// <returns>A list of Refund Documents</returns>
        /// <response code="200">Returns the list of Refund Documents</response>
        /// <response code="400">Status value is invalid</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<RefundDocumentResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByStatus([FromQuery, Required] Status status, [FromQuery] string orderBy="total", [FromQuery] string order="asc")
        {
            _logger.LogInformation($"{nameof(GetByStatus)} requested with 'status' = {status}");

            var documentsByStatus = await _service.GetDocumentsByStatus(status);

            var documentsResponseDtos = await OrderByPreferences(documentsByStatus, orderBy, order);

            _logger.LogInformation($"{nameof(GetByStatus)} responded with body");

            return Ok(documentsResponseDtos);
        }
        /// <summary>
        /// Creates a Refund Document
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/RefundDocs
        ///     {
        ///         "total": 200,
        ///         "categoryId": 2,
        ///         "description": "Document description"
        ///     }
        /// </remarks>
        /// <param name="documentDto">Refund Document request body
        /// <para>The JSON object representing some fields of the Refund Document</para>
        /// </param>
        /// <returns>A new Refund Document</returns>
        /// <response code="201">Returns the newly created Refund Document</response>
        /// <response code="400">At least one field in the Refund Document Request Body is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(RefundDocumentResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [TypeFilter(typeof(ValidationActionFilter))]
        public async Task<IActionResult> PostRequestDoc([FromBody] RefundDocumentRequestDto documentDto)
        {
            _logger.LogInformation($"{nameof(PostRequestDoc)} requested with body");

            var createdDocumentResult = await _service.CreateDocument(documentDto);

            if (!createdDocumentResult.Success)
                return createdDocumentResult.ErrorActionResult!;

            var documentById = await _service.GetDocumentById(createdDocumentResult.Value!.Id);

            if (!documentById.Success)
                return documentById.ErrorActionResult!;

            var documentResponseDto = new RefundDocumentResponseDto(documentById.Value!);

            _logger.LogInformation($"{nameof(PostRequestDoc)} responded with body");

            return Created($"api/refunddocs/{documentResponseDto.Id}", documentResponseDto);
        }

        /// <summary>
        /// Updates a Refund Document by changing its Status to Approved
        /// </summary>
        /// <param name="id">Id of Refund Document to update</param>
        /// <returns></returns>
        /// <response code="204">Refund Document's Status was updated</response>
        /// <response code="400">Id format is invalid</response>
        /// <response code="404">Refund Document not found</response>
        /// <response code="409">Refund Document could not be updated</response>
        [HttpPatch("{id}/approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PatchApprove([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(PatchApprove)} requested with 'id' = {id}");

            var approvedDocumentResult = await _service.ApproveDocument(id);

            if (!approvedDocumentResult.Success)
                return approvedDocumentResult.ErrorActionResult!;

            _logger.LogInformation($"{nameof(PatchApprove)} responded");

            return NoContent();
        }
        /// <summary>
        /// Updates a Refund Document by changing its Status to Disapproved
        /// </summary>
        /// <param name="id">Id of Refund Document to update</param>
        /// <returns></returns>
        /// <response code="204">Refund Document's Status was updated</response>
        /// <response code="400">Id format is invalid</response>
        /// <response code="404">Refund Document not found</response>
        /// <response code="409">Refund Document could not be updated</response>
        [HttpPatch("{id}/disapprove")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PatchDisapprove([FromRoute] Guid id)
        {
            _logger.LogInformation($"{nameof(PatchDisapprove)} requested with 'id' = {id}");

            var disapprovedDocumentResult = await _service.DisapproveDocument(id);

            if (!disapprovedDocumentResult.Success)
                return disapprovedDocumentResult.ErrorActionResult!;

            _logger.LogInformation($"{nameof(PatchDisapprove)} responded");

            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<IEnumerable<RefundDocumentResponseDto>> OrderByPreferences(IEnumerable<RefundDocument> documents, string orderBy, string order)
        {
            Func<RefundDocument, IComparable> orderBySelector;

            if (orderBy == "date")
            {
                orderBySelector = document => document.CreatedAt;
            }
            else
            {
                orderBySelector = document => document.Total;
            }

            Func<IEnumerable<RefundDocument>, IEnumerable<RefundDocument>> documentsFunction;

            if (order == "desc")
            {
                documentsFunction = documents => documents.OrderByDescending(orderBySelector);
            }
            else
            {
                documentsFunction = documents => documents.OrderBy(orderBySelector);
            }

            var documentsOrdered = await Task.FromResult(documentsFunction(documents).ToList());

            var documentsDtosOrdered = await Task.FromResult(documentsOrdered.Select(index => new RefundDocumentResponseDto(index)));

            return documentsDtosOrdered;
        }
    }
}
