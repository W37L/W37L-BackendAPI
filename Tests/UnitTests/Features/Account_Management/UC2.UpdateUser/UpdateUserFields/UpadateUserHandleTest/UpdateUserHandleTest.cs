using UnitTests.Common.Factories;
using UnitTests.Fakes;
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using W3TL.Core.Application.CommandDispatching.Commands.User;
using W3TL.Core.Application.Features.User;
using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;

public class UpdateUserHandlerTests {
    private readonly UpdateUserHandler _handler;
    private readonly FakeUoW _unitOfWork;
    private readonly InMemUserRepoStub _userRepository;

    public UpdateUserHandlerTests() {
        _userRepository = new InMemUserRepoStub();
        _unitOfWork = new FakeUoW();
        _handler = new UpdateUserHandler(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_UserExists_Success() {
        // Arrange - Add a user to the in-memory repository for updating
        var userId = UserID.Create(ValidFields.VALID_USER_ID).Payload;
        var userName = UserNameType.Create(ValidFields.VALID_USERNAME).Payload;
        var firstName = NameType.Create(ValidFields.VALID_FIRST_NAME).Payload;
        var lastName = LastNameType.Create(ValidFields.VALID_LAST_NAME).Payload;
        var bio = BioType.Create(ValidFields.VALID_BIO).Payload;
        var location = LocationType.Create(ValidFields.VALID_LOCATION).Payload;
        var website = WebsiteType.Create(ValidFields.VALID_WEBSITE).Payload;

        var validNameToChange = "UpdatedName";

        var user = UserFactory.Init()
            .WitValidId(ValidFields.VALID_USER_ID)
            .WithValidUserName(ValidFields.VALID_USERNAME)
            .WithValidFirstName(ValidFields.VALID_FIRST_NAME)
            .WithValidLastName(ValidFields.VALID_LAST_NAME)
            .WithValidEmail(ValidFields.VALID_EMAIL)
            .Build();

        await _userRepository.AddAsync(user);

        var command = UpdateUserCommand.Create(userId.Value, validNameToChange, firstName.Value, lastName.Value,
            bio.Value, location.Value, website.Url).Payload;

        // Act - Update the user
        var result = await _handler.HandleAsync(command);

        // Assert - Verify the update was successful
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_Failure() {
        // Arrange - Add a user to the in-memory repository for updating
        var userId = UserID.Create(ValidFields.VALID_USER_ID).Payload;
        var userName = UserNameType.Create(ValidFields.VALID_USERNAME).Payload;
        var firstName = NameType.Create(ValidFields.VALID_FIRST_NAME).Payload;
        var lastName = LastNameType.Create(ValidFields.VALID_LAST_NAME).Payload;
        var bio = BioType.Create(ValidFields.VALID_BIO).Payload;
        var location = LocationType.Create(ValidFields.VALID_LOCATION).Payload;
        var website = WebsiteType.Create(ValidFields.VALID_WEBSITE).Payload;

        var validNameToChange = "UpdatedName";

        var user = UserFactory.Init()
            .WitValidId(ValidFields.VALID_USER_ID)
            .WithValidUserName(ValidFields.VALID_USERNAME)
            .WithValidFirstName(ValidFields.VALID_FIRST_NAME)
            .WithValidLastName(ValidFields.VALID_LAST_NAME)
            .WithValidEmail(ValidFields.VALID_EMAIL)
            .Build();

        var command = UpdateUserCommand.Create(userId.Value, validNameToChange, firstName.Value, lastName.Value,
            bio.Value, location.Value, website.Url).Payload;

        // Act - Attempt to update a non-existing user
        var result = await _handler.HandleAsync(command);

        // Assert - Verify that the update fails
        Assert.True(result.IsFailure);
        Assert.Equal(Error.UserNotFound, result.Error);
    }
}