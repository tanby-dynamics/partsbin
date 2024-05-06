using NSubstitute;
using partsbin.BusinessLogic.Models;
using partsbin.BusinessLogic.Services;
using partsbin.BusinessLogic.Services.PartServices;
using Xunit;

namespace partsbin.Tests.BusinessLogic.Services.PartServices;

public class DuplicatePartTests
{
    [Fact]
    public async Task WhenDuplicating_QuantityIsApproximateIsDuplicated()
    {
        var partService = Substitute.For<IPartService>();
        var sut = new DuplicatePart(
            partService,
            Substitute.For<IImageService>(),
            Substitute.For<IFileService>());

        // test with true
        await sut.ExecuteAsync(new Part
        {
            QuantityIsApproximate = true
        });
        await partService
            .Received()
            .AddPart(Arg.Is<Part>(p => p.QuantityIsApproximate == true));
        
        // test with false
        await sut.ExecuteAsync(new Part
        {
            QuantityIsApproximate = false
        });
        await partService
            .Received()
            .AddPart(Arg.Is<Part>(p => p.QuantityIsApproximate == false));
    }
}