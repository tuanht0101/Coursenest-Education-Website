using AutoMapper;
using CommonLibrary.API.MessageBus.Commands;
using CommonLibrary.API.MessageBus.Responses;
using Identity.API.DTOs;
using Identity.API.Infrastructure.Entities;
using static Identity.API.DTOs.UserAdminResults;

namespace Identity.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		// User
		CreateMap<CreateUser, User>()
			.ForMember(
				dst => dst.Created, opt => opt.MapFrom(
				_ => DateTime.Now))
			.ForMember(
				dst => dst.LastModified, opt => opt.MapFrom(
				_ => DateTime.Now))
			.ForMember(
				dst => dst.InterestedTopics, opt => opt.MapFrom(
				src => src.InterestedTopicIds
					.Select(i => new InterestedTopic() { TopicId = i })));
		CreateMap<UpdateUser, User>()
			.ForMember(
				dst => dst.LastModified, opt => opt.MapFrom(
				_ => DateTime.Now))
			.ForAllMembers(options =>
			{
				options.Condition((source, dstination, member) => member != null);
			});

		CreateProjection<User, UserAdminResult>();
		CreateMap<CredentialResults.CredentialResult, UserAdminResult>();
		CreateMap<CredentialResults.RoleResult, RoleResult>();

		CreateMap<User, UserResult>();
		CreateMap<User, UserProfileResult>();
		CreateMap<User, UserInstructorResult>();

		// Experience
		CreateMap<CreateExperience, Experience>();
		CreateMap<Experience, UserProfileResult.ExperienceResult>();

		// Achievement
		CreateMap<CreateUserAchievement, Achievement>();
		CreateMap<Achievement, UserProfileResult.AchievementResult>();

		// Avatar
		CreateMap<Avatar, ImageResult>()
			.ForMember(
				dst => dst.URI, opt => opt.MapFrom(
				src => $"data:{src.MediaType};base64,{Convert.ToBase64String(src.Data)}"));

		// InterestedTopic
		CreateMap<InterestedTopic, int>()
			.ConvertUsing(x => x.TopicId);

		// FollowedTopic
		CreateMap<FollowedTopic, int>()
			.ConvertUsing(x => x.TopicId);
	}
}
