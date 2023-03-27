using AutoMapper;
using CommonLibrary.API.MessageBus.Responses;
using UserData.API.DTOs;
using UserData.API.Infrastructure.Entities;

namespace UserData.API.MappingProfiles;

public class DefaultProfile : Profile
{
	public DefaultProfile()
	{
		// Enrollment
		CreateProjection<Enrollment, EnrollmentResult>();
		CreateProjection<Enrollment, EnrollmentDetailResult>()
			.ForMember(
				dst => dst.CompletedUnitIds, opt => opt.MapFrom(
				src => src.CompletedUnits.Select(x => x.UnitId)));

		// Submission
		CreateMap<ExamResult, Submission>()
			.ForMember(
				dst => dst.Created, opt => opt.MapFrom(
				_ => DateTime.Now))
			.ForMember(
				dst => dst.Deadline, opt => opt.MapFrom(
				src => DateTime.Now.AddMinutes(src.TimeLimitInMinutes)))
			.ForMember(
				dst => dst.Ended, opt => opt.MapFrom(
				src => DateTime.Now.AddMinutes(src.TimeLimitInMinutes)));
		CreateMap<ExamResult.Question, Question>();
		CreateMap<ExamResult.Choice, Choice>()
			.ForMember(
				dst => dst.IsChosen, opt => opt.MapFrom(
				_ => false));

		CreateMap<GradingSubmission.Review, Review>();

		CreateProjection<Submission, SubmissionResults.Submission>();

		CreateProjection<Submission, SubmissionOngoingResult>();
		CreateProjection<Question, SubmissionOngoingResult.QuestionOngoingResult>();
		CreateProjection<Choice, SubmissionOngoingResult.ChoiceOngoingResult>();

		CreateProjection<Submission, SubmissionDetailResult>();
		CreateProjection<Question, SubmissionDetailResult.QuestionDetailResult>();
		CreateProjection<Review, SubmissionDetailResult.ReviewDetailResult>();
		CreateProjection<Comment, SubmissionDetailResult.CommentDetailResult>();
	}
}
