using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

/// <summary>
///  Represents a command to update the profile banner of a user.
/// </summary>
public class UpdateProfileBannerCommand : Command<UserID>, ICommand<UpdateProfileBannerCommand> {
    
    /// <summary>
    ///  Initializes a new instance of the <see cref="UpdateProfileBannerCommand"/> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="profileBanner"></param>
    private UpdateProfileBannerCommand(UserID userId, BannerType profileBanner) : base(userId) {
        Banner = profileBanner;
    }

    /// <summary>
    ///  The profile banner of the user.
    /// </summary>
    public BannerType Banner { get; }

    /// <summary>
    ///  The number of parameters required to create a <see cref="UpdateProfileBannerCommand"/>.
    /// </summary>
    public static int ParametersCount { get; } = 2;

    /// <summary>
    ///  Creates a new <see cref="UpdateProfileBannerCommand"/> instance if the provided arguments are valid.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Result<UpdateProfileBannerCommand> Create(params object[] args) {
        if (args.Length != ParametersCount)
            return Error.WrongNumberOfParameters;

        var errors = new HashSet<Error>();

        var userId = UserID.Create(args[0].ToString())
            .OnFailure(error => errors.Add(error));

        var profileBanner = BannerType.Create(args[1].ToString())
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.CompileErrors(errors);

        return new UpdateProfileBannerCommand(userId.Payload, profileBanner.Payload);
    }
}