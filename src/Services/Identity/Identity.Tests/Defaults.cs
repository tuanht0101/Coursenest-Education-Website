using Identity.API.Infrastructure.Contexts;
using Identity.API.Infrastructure.Entities;

namespace Identity.Tests;

public static class Defaults
{
	public static readonly Action<DataContext> Database = context =>
	{
		context.AddRange(new[]
		{
			new User()
			{
				Email = "one@gmail.com",
				Phonenumber= "0123456789",
				UserId = 1,
				FullName = "Elizabeth",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Female,
				Title= "Developer",
				DateOfBirth= DateTime.Now,
				Location = "Hanoi",
				AboutMe = "Corazon Bryant always had an artistic bone in her body. When she was only 2 years old, she would make detailed drawings on her mother's wall. Her mother loved the drawings so much, she decided to keep them"
			},
			new User() {
				Email = "two@gmail.com",
				Phonenumber = "0123456781",
				UserId = 2,
				FullName = "Mona",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Female,
				Title = "Stdent",
				DateOfBirth = DateTime.Now,
				Location = "Ho Chi Minh City",
				AboutMe = "hello Im Mona, like to meet u!"
			},
			new User()
			{
				Email = "three@gmail.com",
				UserId = 3,
				FullName = "Emma",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Female
			},
			new User()
			{
				Email = "four@gmail.com",
				UserId = 4,
				FullName = "Jessica",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Female
			},
			new User()
			{
				Email = "five@gmail.comstd",
				UserId = 5,
				FullName = "Brian",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Male
			},
			new User()
			{
				Email = "six@gmail.com",
				UserId = 6,
				FullName = "Christopher",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Male
			},
			new User()
			{
				Email = "sevent@gmail.com",
				UserId = 7,
				FullName = "David",
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				Gender = Gender.Male
			}
		});

		context.AddRange(new[]
		{
			new Experience()
			{
				ExperienceId = 1,
				Name = " Hanoi University",
				Title = "Professor",
				Started = DateTime.Now,
				UserId = 1
			},
			new Experience()
			{
				ExperienceId = 2,
				Name = " BBOLD Talent",
				Title = "Employee",
				Started = DateTime.Now,
				UserId = 2
			},
			new Experience()
			{
				ExperienceId = 3,
				Name = " SLEEP TRADING COMPANY LIMITED",
				Title = "Director",
				Started = DateTime.Now,
				UserId = 3
			},
			new Experience()
			{
				ExperienceId = 4,
				Name = " XYZ TECHNOLOGY COMPANY LIMITED",
				Title = "Manager",
				Started = DateTime.Now,
				UserId = 4
			},
			new Experience()
			{
				ExperienceId = 5,
				Name = " ABC INVESTMENT COMPANY LIMITED",
				Title = "Employee",
				Started = DateTime.Now,
				UserId = 5
			},
			new Experience()
			{
				ExperienceId = 6,
				Name = " Hanoi University of science and technology",
				Title = "Engineer",
				Started = DateTime.Now,
				UserId = 6
			},
			new Experience()
			{
				ExperienceId = 7,
				Name = " Ho Chi Minh University",
				Title = "Bachelor",
				Started = DateTime.Now,
				UserId = 7
			}
		});

		context.AddRange(new[]
		{
			new InterestedTopic() { UserId = 1, TopicId = 1 },
			new InterestedTopic() { UserId = 1, TopicId = 4 },
			new InterestedTopic() { UserId = 2, TopicId = 3 },
			new InterestedTopic() { UserId = 3, TopicId = 2 },
			new InterestedTopic() { UserId = 3, TopicId = 3 },
			new InterestedTopic() { UserId = 5, TopicId = 2 },
			new InterestedTopic() { UserId = 7, TopicId = 1 }
		});

		context.AddRange(new[]
		{
			new FollowedTopic() { UserId = 3, TopicId = 2 },
			new FollowedTopic() { UserId = 6, TopicId = 1 },
		});

		context.AddRange(new[]
		{
			new Achievement()
			{
				AchievementId = 1,
				Title = "Learn Java in 1 hour",
				Created = DateTime.Now,
				UserId = 1
			},
			new Achievement()
			{
				AchievementId = 2,
				Title = "Complete Guide to Realistic Character Creation in Blender",
				Created = DateTime.Now,
				UserId = 1
			},
			new Achievement()
			{
				AchievementId = 3,
				Title = "Complete Blender Megacourse: Beginner to Expert",
				Created = DateTime.Now,
				UserId = 1
			},
			new Achievement()
			{
				AchievementId = 4,
				Title = "Microsoft Excel - Excel from Beginner to Advanced",
				Created = DateTime.Now,
				UserId = 2
			},
			new Achievement()
			{
				AchievementId = 5,
				Title = "Zero to Hero in Microsoft Excel: Complete Excel guide 2023",
				Created = DateTime.Now,
				UserId = 2
			},
			new Achievement()
			{
				AchievementId = 6,
				Title = "Data Analyst Skillpath: Zero to Hero in Excel, SQL & Python",
				Created = DateTime.Now,
				UserId = 4
			}
		});
	};
}
