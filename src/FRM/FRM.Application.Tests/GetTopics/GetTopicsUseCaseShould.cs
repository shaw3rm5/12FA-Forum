using FluentValidation;
using FluentValidation.Results;
using Forum.Application.UseCases.GetTopics;
using Moq;

namespace FRM.Application.Tests.GetTopics;

public class GetTopicsUseCaseShould
{

    private readonly GetTopicsUseCase _sut;
    private readonly Mock<IGetTopicsStorage> storage;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsCommand>>();
        validator.Setup(c => c.ValidateAsync(It.IsAny<GetTopicsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        storage = new Mock<IGetTopicsStorage>();
        storage.Setup(c => c.GetTopicsAsync(
            It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        _sut = new GetTopicsUseCase(validator.Object, storage.Object);
    }
    
    [Fact]
    public async Task ReturnTopicsExtractedFromDatabase()
    {
        
    }
}