using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using EventAssociation.Presentation.WebAPI.EndPoints.Event.DTOs.Response;

namespace UnitTests.Endpoint
{
    public class ApiEndpointsIntegrationTests
        : IClassFixture<WebApplicationFactory<EventAssociation.Presentation.WebAPI.Program>>
    {
        private readonly HttpClient _client;

        public ApiEndpointsIntegrationTests(WebApplicationFactory<EventAssociation.Presentation.WebAPI.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "CreateEvent returns 200 OK and a valid EventDto")]
        public async Task CreateEvent_ReturnsValidEventDto()
        {
            var response = await _client.PostAsJsonAsync("/api/CreateEvent", new { });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var createResp = await response.Content.ReadFromJsonAsync<CreateEventResponse>();
            Assert.NotNull(createResp);
            Assert.NotNull(createResp.eventDto);
            Assert.False(string.IsNullOrWhiteSpace(createResp.eventDto.Id));
            Assert.Equal(string.Empty, createResp.eventDto.Title);
            Assert.Equal(string.Empty, createResp.eventDto.Description);
            Assert.Equal(0, createResp.eventDto.MaxGuests);
        }

        [Fact(DisplayName = "GetEventById returns 200 OK when event exists")]
        public async Task GetEventById_ValidId_Returns200()
        {
            var createResp = await _client.PostAsJsonAsync("/api/CreateEvent", new { });
            var createdObj = await createResp.Content.ReadFromJsonAsync<CreateEventResponse>();
            Assert.NotNull(createdObj);
            Assert.NotNull(createdObj.eventDto);
            var id = createdObj.eventDto.Id;

            var getResp = await _client.GetAsync($"/api/GetEventById/{id}");
            Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);

            var dtoResp = await getResp.Content.ReadFromJsonAsync<GetEventByIdResponse>();
            Assert.NotNull(dtoResp);
            Assert.NotNull(dtoResp.eventDto);
            Assert.Equal(id, dtoResp.eventDto.Id);
        }

        [Fact(DisplayName = "GetEventById returns 400 BadRequest when ID is invalid")]
        public async Task GetEventById_InvalidGuid_Returns400()
        {
            var badId = "not-a-guid";
            var getResp = await _client.GetAsync($"/api/GetEventById/{badId}");
            Assert.Equal(HttpStatusCode.BadRequest, getResp.StatusCode);
        }
    }
}