using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealEstate.Core.Constants;
using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;
using RealEstate.Core.Exceptions;
using RealEstate.Core.Interfaces.Services;
using RealEstate.Core.Interfaces.Utils;
using RealEstate.Core.QueryFilters;
using RealEstate.Infraestructure.Filter;
using System.Net;

namespace RealEstate.API.Controllers
{

    /// <summary>
    /// Controller for managing properties, including adding, updating, and retrieving property information.
    /// Handles property images, property details, and pagination for property listings.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [AuthorizeFilter]
    [LogActionFilter]
    public class PropertyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IPropertyService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyController"/> class.
        /// </summary>
        /// <param name="mapper">AutoMapper service for object mapping.</param>
        /// <param name="fileService">Service for handling property-related files and images.</param>
        /// <param name="service">Service for property operations (e.g., create, update).</param>
        public PropertyController(IMapper mapper, IFileService fileService, IPropertyService service)
        {
            _mapper = mapper;
            _fileService = fileService;
            _service = service;
        }

        /// <summary>
        /// Adds a new property.
        /// </summary>
        /// <param name="request">The property creation request containing necessary details.</param>
        /// <returns>Created property information or an error if something went wrong.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddProperty([FromBody] PropertyCreationRequestDto request)
        {
            try
            {
                await _fileService.PropertySaveMultipleImagesAsync(request);
                var entity = _mapper.Map<Property>(request);
                var idProperty = await _service.AddProperty(entity);
                return CreatedAtAction(nameof(GetById), new { id = idProperty }, new { Message = "Property successfully created" });
            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, ex.Message + ex.InnerException, MessageConstant.PROPERTY_CREATE_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Updates an existing property by ID.
        /// </summary>
        /// <param name="id">The ID of the property to update.</param>
        /// <param name="request">The update request with new property details.</param>
        /// <returns>Updated property information or an error if something went wrong.</returns>
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProperty(long id, [FromBody] PropertyUpdateRequestDto request)
        {
            try
            {
                if (id != request.IdProperty)
                {
                    return BadRequest();
                }

                var property = await _service.GetById(id);

                if (property != null)
                {
                    property.Address = request.Address;
                    property.Name = request.Name;
                    property.Price = request.Price;
                    property.Active = request.Active;
                    property.CodeInternal = request.CodeInternal;
                    property.IdOwner = request.IdOwner;
                    property.Year = request.Year;

                    var idProperty = _service.UpdateProperty(property);
                    return CreatedAtAction(nameof(GetById), new { id = idProperty }, new { Message = "Property successfully updated" });
                }
                else
                {
                    return NotFound(new { Message = MessageConstant.PROPERTY_NOT_EXISTS_ERROR_MESSAGE.Replace("ID_PROPERTY", id.ToString()) });
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, ex.Message + ex.InnerException, MessageConstant.PROPERTY_CREATE_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Retrieves property details by ID.
        /// </summary>
        /// <param name="id">The ID of the property to retrieve.</param>
        /// <returns>Property details or an error if something went wrong.</returns>
        [HttpGet("get-byid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(long id)
        {
            PropertyDto result;

            try
            {
                var property = await _service.GetById(id);

                if (property != null)
                {
                    result = _mapper.Map<PropertyDto>(property);
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, MessageConstant.PROPERTY_GETBYID_ERROR_MESSAGE, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a paginated list of properties based on filters.
        /// </summary>
        /// <param name="filters">Query filters for properties, such as page size, search term, etc.</param>
        /// <returns>Paginated list of properties or an error if something went wrong.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> All([FromQuery] PropertyQueryFilter filters)
        {
            try
            {
                var dataResult = await _service.GetPaginated(filters);
                var infoResponse = _mapper.Map<IEnumerable<PropertyDto>>(dataResult);

                var metadata = new
                {
                    dataResult.TotalCount,
                    dataResult.PageSize,
                    dataResult.CurrentPage,
                    dataResult.TotalPages,
                    dataResult.HasPreviousPage,
                    dataResult.HasNextPage,
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(new
                {
                    data = infoResponse,
                    metadata
                });
            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, MessageConstant.PROPERTY_PAGINATED_ERROR_MESSAGE, ex.Message);
            }
        }

        /// <summary>
        /// Adds an image to a property.
        /// </summary>
        /// <param name="id">The ID of the property to add an image to.</param>
        /// <param name="request">The image creation request containing image details.</param>
        /// <returns>Success message or an error if something went wrong.</returns>
        [HttpPut("add-image/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddImageProperty(long id, [FromBody] PropertyImageCreationRequestDto request)
        {
            try
            {
                if (id != request.IdProperty)
                {
                    return BadRequest();
                }

                var property = await _service.GetById(id);

                if (property != null)
                {
                    var entity = _mapper.Map<PropertyImage>(request);
                    entity.FileUrl = await _fileService.PropertySaveAImagesAsync(request, property.CodeInternal);
                    await _service.AddImageProperty(entity);
                    return Ok(new { Message = MessageConstant.PROPERTY_IMAGE_CREATE_OK_MESSAGE });
                }
                else
                {
                    return NotFound(new { Message = MessageConstant.PROPERTY_NOT_EXISTS_ERROR_MESSAGE.Replace("ID_PROPERTY", id.ToString()) });
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(HttpStatusCode.BadRequest, ex.Message + ex.InnerException, MessageConstant.PROPERTY_CREATE_ERROR_MESSAGE);
            }
        }
    }
}
