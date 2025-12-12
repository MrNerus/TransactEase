using Microsoft.AspNetCore.Mvc;
using Moq;
using TransactEase.BusinessLayer;
using TransactEase.Controllers;
using TransactEase.DataLayer;
using TransactEase.DTO;
using TransactEase.Enums;
using TransactEase.Models;

namespace MSTest_Unit.UnitTest;

[TestClass]
public class OrganizationControllerTests
{
    private Mock<OrganizationHandler> _organizationHandlerMock;
    private OrganizationController _organizationController;

    [TestInitialize]
    public void TestInitialize()
    {
        var organizationDalMock = new Mock<OrganizationDAL>(new DbConnectionModel());
        _organizationHandlerMock = new Mock<OrganizationHandler>(organizationDalMock.Object, new DbConnectionModel());
        _organizationController = new OrganizationController(_organizationHandlerMock.Object);
    }

    [TestMethod]
    public async Task CreateOrganization_ReturnsOkResult_WhenRequestIsValid()
    {
        // Arrange
        var organization = new OrganizationModel
        {
            Id = "1",
            Code = "ORG001",
            Name = "Test Organization",
            Address = "123 Test St",
            SwiftCode = "TESTSWIFT",
            Country = "Testland",
            Type = OrganizationTypeEnum.Organization,
            Contact = "123-456-7890",
            Email = "test@test.com"
        };

        var userResponse = new UserResponse { Status = StatusEnum.SUCCESS };

        _organizationHandlerMock.Setup(x => x.CreateOrganizationAsync(It.IsAny<OrganizationModel>()))
            .ReturnsAsync(userResponse);

        // Act
        var result = await _organizationController.CreateOrganization(organization);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetAllOrganizations_ReturnsOkResult()
    {
        // Arrange
        var userResponse = new UserResponse { Status = StatusEnum.SUCCESS };
        _organizationHandlerMock.Setup(x => x.GetAllOrganizationsAsync()).ReturnsAsync(userResponse);

        // Act
        var result = await _organizationController.GetAllOrganizations();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetOrganizationById_ReturnsOkResult_WhenOrganizationExists()
    {
        // Arrange
        var userResponse = new UserResponse { Status = StatusEnum.SUCCESS };
        _organizationHandlerMock.Setup(x => x.GetOrganizationByIdAsync(It.IsAny<string>())).ReturnsAsync(userResponse);

        // Act
        var result = await _organizationController.GetOrganizationById("1");

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task UpdateOrganization_ReturnsOkResult_WhenRequestIsValid()
    {
        // Arrange
        var organization = new OrganizationModel
        {
            Id = "1",
            Code = "ORG001",
            Name = "Test Organization",
            Address = "123 Test St",
            SwiftCode = "TESTSWIFT",
            Country = "Testland",
            Type = OrganizationTypeEnum.Organization,
            Contact = "123-456-7890",
            Email = "test@test.com"
        };

        var userResponse = new UserResponse { Status = StatusEnum.SUCCESS };

        _organizationHandlerMock.Setup(x => x.UpdateOrganizationAsync(It.IsAny<string>(), It.IsAny<OrganizationModel>()))
            .ReturnsAsync(userResponse);

        // Act
        var result = await _organizationController.UpdateOrganization("1", organization);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task DeleteOrganization_ReturnsOkResult()
    {
        // Arrange
        var userResponse = new UserResponse { Status = StatusEnum.SUCCESS };
        _organizationHandlerMock.Setup(x => x.DeleteOrganizationAsync(It.IsAny<string>())).ReturnsAsync(userResponse);

        // Act
        var result = await _organizationController.DeleteOrganization("1");

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }
}
