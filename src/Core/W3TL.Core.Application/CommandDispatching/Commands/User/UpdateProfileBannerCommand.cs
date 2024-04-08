using W3TL.Core.Domain.Agregates.User.Entity.Values;
using W3TL.Core.Domain.Common.Values;

namespace W3TL.Core.Application.CommandDispatching.Commands.User;

public class UpdateProfileBannerCommand : Command<UserID>, ICommand<UpdateProfileBannerCommand> {
    private UpdateProfileBannerCommand(UserID userId, BannerType profileBanner) : base(userId) {
        Banner = profileBanner;
    }

    public BannerType Banner { get; }

    public static int ParametersCount { get; } = 2;

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