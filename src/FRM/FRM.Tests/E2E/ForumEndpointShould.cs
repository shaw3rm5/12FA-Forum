using System.Net.Http.Json;
using FluentAssertions;
using FRM.API.Dtos;

namespace FRM.Tests.E2E
{
    public class ForumEndpointShould : IClassFixture<ForumApiApplicationFactory>
    {
        private readonly ForumApiApplicationFactory _factory;

        public ForumEndpointShould(ForumApiApplicationFactory factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task CreateForum()
        {
        
            using var client = _factory.CreateClient();
            
            using var initialForumResponse = await client.GetAsync("/Forum");
       
            initialForumResponse.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            initialForumResponse.EnsureSuccessStatusCode();
       
            var initialResult = await initialForumResponse.Content.ReadFromJsonAsync<ForumDto[]>();

            initialResult!
                .Should().BeEmpty();
            
            using var response = await client.PostAsync("/forum", JsonContent.Create(new
            {
                Title = "GOVNOSOS"
            }));
            
            response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            response.EnsureSuccessStatusCode();
            
            var forum = await response.Content.ReadFromJsonAsync<ForumDto>();
            
            forum!
                .Title.Should().Be("GOVNOSOS");
            
            
            // using var getForumsResponse = await client.GetAsync("/Forum");
            //
            // getForumsResponse.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();
            // getForumsResponse.EnsureSuccessStatusCode();
            //
            // var result = await getForumsResponse.Content.ReadFromJsonAsync<ForumDto[]>();
            //
            // result!
            //     .Should().Contain(r => r.Title == "GOVNOSOS");
        }
        
    }
}
