using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;
using Xunit.Abstractions;
using static UnitTests.Common.Factories.ValidFields;

public class UserCreationTestsSuccess {
    private readonly ITestOutputHelper _testOutputHelper;

    public UserCreationTestsSuccess(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    //ID:UC1.S1
    [Fact]
    public void CreateUser_TestTheValidConstants_ReturnSuccess() {
        //Arrange
        var userId = UserID.Create(VALID_USER_ID).Payload;
        var userName = UserNameType.Create(VALID_USERNAME).Payload;
        var firstName = NameType.Create(VALID_FIRST_NAME).Payload;
        var lastName = LastNameType.Create(VALID_LAST_NAME).Payload;
        var email = EmailType.Create(VALID_EMAIL).Payload;
        var pub = PubType.Create(VALID_PUB_KEY).Payload;
        var createdAt = CreatedAtType.Create(VALID_CREATED_AT_UNIX).Payload;

        //Act
        var result = User.Create(userId, userName, firstName, lastName, email, pub);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.IsType<UserID>(result.Payload.Id);
        Assert.Equal(VALID_USERNAME, result.Payload.UserName.Value);
        Assert.Equal(VALID_FIRST_NAME, result.Payload.FirstName.Value);
        Assert.Equal(VALID_LAST_NAME, result.Payload.LastName.Value);
        Assert.Equal(VALID_EMAIL, result.Payload.Email.Value);
        Assert.Equal(VALID_PUB_KEY, result.Payload.Pub.Value);
        Assert.IsType<long>(result.Payload.CreatedAt.Value);
    }

    // //ID:UC1.S2
    // [Theory]
    // [InlineData("UID-123456789012345678901234567890123456")]
    // [InlineData("UID-123456789012345678901234567890123457")]
    // [InlineData("UID-123456789012345678901234567890123458")]
    // public void CreateUser_CorrectUserID_ReturnSuccess(string userIdInput) {
    //     //Arrange
    //     var userId = UserID.Create(userIdInput).Value;
    //
    //     //Act
    //     var result = User.Create(userId, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);
    //
    //     //Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(userIdInput, result.Value.Id.Value);
    // }

    //ID:UC1.S3
    [Theory]
    [InlineData("ValidUsername1")]
    [InlineData("Username2")]
    [InlineData("12353")]
    public void CreateUser_CorrectUsername_ReturnSuccess(string? username) {
        //Arrange
        var userName = UserNameType.Create(username).Payload;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Payload,
            userName, NameType.Create(VALID_FIRST_NAME).Payload, LastNameType.Create(VALID_LAST_NAME).Payload,
            EmailType.Create(VALID_EMAIL).Payload, PubType.Create(VALID_PUB_KEY).Payload);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(username, result.Payload.UserName.Value);
    }

    //ID:UC1.S4
    [Theory]
    [InlineData("email@example.com")]
    [InlineData("another.email@example.co.uk")]
    [InlineData("user123@domain.com")]
    public void CreateUser_CorrectEmail_ReturnSuccess(string? emailInput) {
        //Arrange
        var email = EmailType.Create(emailInput).Payload;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Payload, UserNameType.Create(VALID_USERNAME).Payload,
            NameType.Create(VALID_FIRST_NAME).Payload, LastNameType.Create(VALID_LAST_NAME).Payload, email,
            PubType.Create(VALID_PUB_KEY).Payload);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(emailInput, result.Payload.Email.Value);
    }

    //ID:UC1.S5
    [Theory]
    [InlineData("John")]
    [InlineData("Jane")]
    [InlineData("Alice")]
    public void CreateUser_CorrectFirstName_ReturnSuccess(string? firstName) {
        //Arrange
        var name = NameType.Create(firstName).Payload;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Payload, UserNameType.Create(VALID_USERNAME).Payload,
            name, LastNameType.Create(VALID_LAST_NAME).Payload, EmailType.Create(VALID_EMAIL).Payload,
            PubType.Create(VALID_PUB_KEY).Payload);
        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(firstName, result.Payload.FirstName.Value);
    }

    //ID:UC1.S6
    [Theory]
    [InlineData("Doe")]
    [InlineData("Smith")]
    [InlineData("Johnson")]
    public void CreateUser_CorrectLastName_ReturnSuccess(string? lastName) {
        //Arrange
        var name = LastNameType.Create(lastName).Payload;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Payload,
            UserNameType.Create(VALID_USERNAME).Payload, NameType.Create(VALID_FIRST_NAME).Payload, name,
            EmailType.Create(VALID_EMAIL).Payload, PubType.Create(VALID_PUB_KEY).Payload);
        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(lastName, result.Payload.LastName.Value);
    }

    //ID:UC1.S7
    [Theory]
    [InlineData("FJbAsEj5pj+xm0lMRwqym72lPWOU0NNeFxwId+bC1iF=")]
    [InlineData("FJbAsEj5pj+xm0lM123ym72lPWOU0NNeFxwId+bC1iF=")]
    [InlineData("FJbAsEj5pj+xm0lMRwqym71234560NNeFxwId+bC1iF=")]
    public void CreateUser_CorrectPubKey_ReturnSuccess(string? pubKey) {
        //Arrange
        var pub = PubType.Create(pubKey).Payload;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Payload,
            UserNameType.Create(VALID_USERNAME).Payload, NameType.Create(VALID_FIRST_NAME).Payload,
            LastNameType.Create(VALID_LAST_NAME).Payload, EmailType.Create(VALID_EMAIL).Payload, pub);
        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
        Assert.Equal(pubKey, result.Payload.Pub.Value);
    }

    // //ID:UC1.S8
    // [Theory]
    // [InlineData(1622548800)]
    // [InlineData(1622548801)]
    // [InlineData(1622548802)]
    // public void CreateUser_CorrectCreatedAt_ReturnSuccess(long createdAt) {
    //     //Arrange
    //     var created = CreatedAtType.Create(createdAt).Value;
    //
    //     //Act
    //     var result = User.Create(UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, created);
    //     // Verification
    //     Assert.True(result.IsSuccess);
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(createdAt, result.Value.CreatedAt.Value);
    // }

    // //ID:UC1.S9
    // [Theory]
    // [InlineData("2023-03-25T13:45:30")]
    // [InlineData("2019-12-25T23:45:31")]
    // [InlineData("1999-01-02T01:45:32")]
    // public void CreateUser_CorrectCreatedAtString_ReturnSuccess(string createdAtString) {
    //     //Arrange
    //     var created = CreatedAtType.Create(createdAtString).Value;
    //
    //
    //     //Act
    //     var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, created);
    //
    //     // Verification
    //     Assert.True(result.IsSuccess);
    //     Assert.NotNull(result.Value);
    //     Assert.Equal(created.Value, result.Value.CreatedAt.Value);
    //
    // }
}