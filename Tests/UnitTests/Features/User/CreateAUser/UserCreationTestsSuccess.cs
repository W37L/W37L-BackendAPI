
using ViaEventAssociation.Core.Domain.Aggregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Values;
using W3TL.Core.Domain.Agregates.User;
using W3TL.Core.Domain.Agregates.User.Values;
using W3TL.Core.Domain.Common.Values;
using Xunit.Abstractions;

public class UserCreationTestsSuccess {
    private readonly ITestOutputHelper _testOutputHelper;
    public UserCreationTestsSuccess(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }

    private const string VALID_USER_ID = "UID-123456789012345678901234567890123456";
    private const string VALID_USERNAME = "something_valid";
    private const string VALID_FIRST_NAME = "John";
    private const string VALID_LAST_NAME = "Doe";
    private const string VALID_EMAIL = "john.doe@example.com";
    private const string VALID_PUB_KEY = "FJbAsEj5pj+xm0lMRwqym72lPWOU0NNeFxwId+bC1iF=";
    private const long VALID_CREATED_AT_UNIX = 1622548800;

    //ID:UC1.S1
    [Fact]
    public void CreateUser_TestTheValidConstansts_ReturnSuccess() {
        //Arrange
        var userId = UserID.Create(VALID_USER_ID).Value;
        var userName = UserNameType.Create(VALID_USERNAME).Value;
        var firstName = NameType.Create(VALID_FIRST_NAME).Value;
        var lastName = LastNameType.Create(VALID_LAST_NAME).Value;
        var email = EmailType.Create(VALID_EMAIL).Value;
        var pub = PubType.Create(VALID_PUB_KEY).Value;
        var createdAt = CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value;

        //Act
        var result = User.Create(userId, userName, firstName, lastName, email, pub, createdAt);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(VALID_USER_ID, result.Value.Id.Value);
        Assert.Equal(VALID_USERNAME, result.Value.UserName.Value);
        Assert.Equal(VALID_FIRST_NAME, result.Value.FirstName.Value);
        Assert.Equal(VALID_LAST_NAME, result.Value.LastName.Value);
        Assert.Equal(VALID_EMAIL, result.Value.Email.Value);
        Assert.Equal(VALID_PUB_KEY, result.Value.Pub.Value);
        Assert.Equal(VALID_CREATED_AT_UNIX, result.Value.CreatedAt.Value);

    }

    //ID:UC1.S2
    [Theory]
    [InlineData("UID-123456789012345678901234567890123456")]
    [InlineData("UID-123456789012345678901234567890123457")]
    [InlineData("UID-123456789012345678901234567890123458")]
    public void CreateUser_CorrectUserID_ReturnSuccess(string userIdInput) {
        //Arrange
        var userId = UserID.Create(userIdInput).Value;

        //Act
        var result = User.Create(userId, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userIdInput, result.Value.Id.Value);
    }

    //ID:UC1.S3
    [Theory]
    [InlineData("ValidUsername1")]
    [InlineData("Username2")]
    [InlineData("12353")]
    public void CreateUser_CorrectUsername_ReturnSuccess(string username) {
        //Arrange
        var userName = UserNameType.Create(username).Value;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, userName, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(username, result.Value.UserName.Value);
    }

    //ID:UC1.S4
    [Theory]
    [InlineData("email@example.com")]
    [InlineData("another.email@example.co.uk")]
    [InlineData("user123@domain.com")]
    public void CreateUser_CorrectEmail_ReturnSuccess(string emailInput) {
        //Arrange
        var email = EmailType.Create(emailInput).Value;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, email, PubType.Create(VALID_PUB_KEY).Value, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(emailInput, result.Value.Email.Value);

    }

    //ID:UC1.S5
    [Theory]
    [InlineData("John")]
    [InlineData("Jane")]
    [InlineData("Alice")]
    public void CreateUser_CorrectFirstName_ReturnSuccess(string firstName) {
        //Arrange
        var name = NameType.Create(firstName).Value;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, name, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(firstName, result.Value.FirstName.Value);
    }

    //ID:UC1.S6
    [Theory]
    [InlineData("Doe")]
    [InlineData("Smith")]
    [InlineData("Johnson")]
    public void CreateUser_CorrectLastName_ReturnSuccess(string lastName) {
        //Arrange
        var name = LastNameType.Create(lastName).Value;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, name, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(lastName, result.Value.LastName.Value);
    }

    //ID:UC1.S7
    [Theory]
    [InlineData("FJbAsEj5pj+xm0lMRwqym72lPWOU0NNeFxwId+bC1iF=")]
    [InlineData("FJbAsEj5pj+xm0lM123ym72lPWOU0NNeFxwId+bC1iF=")]
    [InlineData("FJbAsEj5pj+xm0lMRwqym71234560NNeFxwId+bC1iF=")]
    public void CreateUser_CorrectPubKey_ReturnSuccess(string pubKey) {
        //Arrange
        var pub = PubType.Create(pubKey).Value;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, pub, CreatedAtType.Create(VALID_CREATED_AT_UNIX).Value);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(pubKey, result.Value.Pub.Value);
    }

    //ID:UC1.S8
    [Theory]
    [InlineData(1622548800)]
    [InlineData(1622548801)]
    [InlineData(1622548802)]
    public void CreateUser_CorrectCreatedAt_ReturnSuccess(long createdAt) {
        //Arrange
        var created = CreatedAtType.Create(createdAt).Value;

        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, created);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(createdAt, result.Value.CreatedAt.Value);
    }

    //ID:UC1.S9
    [Theory]
    [InlineData("2023-03-25T13:45:30")]
    [InlineData("2019-12-25T23:45:31")]
    [InlineData("1999-01-02T01:45:32")]
    public void CreateUser_CorrectCreatedAtString_ReturnSuccess(string createdAtString) {
        //Arrange
        var created = CreatedAtType.Create(createdAtString).Value;


        //Act
        var result = User.Create(UserID.Create(VALID_USER_ID).Value, UserNameType.Create(VALID_USERNAME).Value, NameType.Create(VALID_FIRST_NAME).Value, LastNameType.Create(VALID_LAST_NAME).Value, EmailType.Create(VALID_EMAIL).Value, PubType.Create(VALID_PUB_KEY).Value, created);

        // Verification
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(created.Value, result.Value.CreatedAt.Value);

    }
}
