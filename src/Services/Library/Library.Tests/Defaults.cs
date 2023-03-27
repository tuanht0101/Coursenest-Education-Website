using Library.API.Infrastructure.Contexts;
using Library.API.Infrastructure.Entities;

namespace Library.Tests;

public static class Defaults
{
	public static readonly Action<DataContext> CategoriesDatabase = context =>
	{
		context.AddRange(new[]
		{
			new Category()
			{
				CategoryId = 1,
				Content = "Development",
				Subcategories = new()
				{
					new Subcategory()
					{
						Content = "Web Development",
						Topics = new()
						{
							new Topic() { Content = "JavaScript" },
							new Topic() { Content = "CSS" }
						}
					},
					new Subcategory()
					{
						Content = "Data Science",
						Topics = new()
						{
							new Topic() { Content = "Python" },
							new Topic() { Content = "Machine Learning" }
						}
					}
				}
			},
			new Category()
			{
				CategoryId = 2,
				Content = "Business",
				Subcategories = new()
				{
					new Subcategory()
					{
						Content = "Entrepreneurship",
						Topics = new()
						{
							new Topic() { Content = "Business Fundamentals" },
							new Topic() { Content = "Freelancing" }
						}
					},
					new Subcategory()
					{
						Content = "Communication",
						Topics = new()
						{
							new Topic() { Content = "Communication" },
							new Topic() { Content = "Public Speaking" }
						}
					}
				}
			},
			new Category() { CategoryId = 3, Content = "Finance & Accounting" },
			new Category() { CategoryId = 4, Content = "IT & Software" }
		});
	};

	public static readonly Action<DataContext> Database = context =>
	{
		context.AddRange(new[]
		{
			new Course()
			{
				CourseId = 1,
				Title = "Learn Java",
				Description = "How to learn Java in 10 hours",
				About = "I will teach...",
				Tier = CourseTier.Free,
				IsApproved = false,
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				TopicId = 1,
				PublisherUserId = 1,
				RatingAverage = 3,
				RatingTotal = 10,
				Lessons = new()
				{
					new Lesson()
					{
						LessonId = 1,
						Title = "Lesson 1: How to set up Visual studio",
						Description = " The way to set up is so easy",
						OrderNumerator = 1,
						OrderDenominator = 1,
						Units= new()
						{
							new Material()
							{
								Title = "Material 1",
								RequiredTime = TimeSpan.FromMinutes(15),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Content = "Check the system requirements. These requirements help you know whether your computer supports Visual Studio 2022",
							 },

							new Material()
							{
								Title = "Material 2",
								RequiredTime = TimeSpan.FromMinutes(15),
								OrderNumerator = 2 ,
								OrderDenominator = 1,
								Content = "Apply the latest Windows updates. These updates ensure that your computer has both the latest security updates and the required system components for Visual Studio.",
							 }
						}
					},

					new Lesson()
					{
						Title = "Lesson 2: Java Syntax",
						Description = " In the previous chapter, we created a Java file called Main.java, and we used the following code to print \"Hello World\" to the screen",
						OrderNumerator = 2,
						OrderDenominator = 1
					},
					 new Lesson()
					{
						Title = "Lesson 3: Java Comments",
						Description = " Comments can be used to explain Java code, and to make it more readable. It can also be used to prevent execution when testing alternative code.",
						OrderNumerator = 3,
						OrderDenominator = 1,
						Units= new()
						{
							new Exam()
							{
								Title = "Exam 1",
								RequiredTime = TimeSpan.FromMinutes(1),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Questions=new()
								{
									new Question()
									{
									QuestionId = 1,
									Point = 2,
									ExamUnitId = 1,
									Content = " 1. How do you insert COMMENTS in Java code?",
									Choices = new()
									{
										new Choice()
										{
											ChoiceId = 1,
											IsCorrect = true,
											Content = "// this is a comment"
										},
										new Choice()
										{
											ChoiceId = 2,
											IsCorrect = false,
											Content = "# this is a comment"
										}
									}

									},
									new Question()
									{
									QuestionId = 2,
									Point = 2,
									ExamUnitId = 1,
									Content = " 2. Which data type is used to create a variable that should store text?",
									Choices = new()
									{
										new Choice()
										{
											ChoiceId = 1,
											IsCorrect = true,
											Content = "a. String"
										},
										new Choice()
										{
											ChoiceId = 2,
											IsCorrect = false,
											Content = "b. Mystring"
										},
										new Choice()
										{
											ChoiceId = 3,
											IsCorrect = false,
											Content = "c. txt"
										}
									}

									}

								}
							}
						}
					}
				}

			},
			new Course()
			{
				CourseId = 2,
				Title = "Learn C++",
				Description = "How to learn C++ in 10 hours",
				About = "I will teach...",
				Tier = CourseTier.Free,
				IsApproved = false,
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				TopicId = 2,
				PublisherUserId = 2,
				RatingAverage = 3,
				RatingTotal = 20,
				Lessons = new()
				{
					new Lesson()
					{
						LessonId = 1,
						Title = "Lesson 1: C++ Intro",
						Description = " What is C++?\r\nC++ is a cross-platform language that can be used to create high-performance applications.",
						OrderNumerator = 1,
						OrderDenominator = 1,
					},
					  new Lesson()
					{
						LessonId = 2,
						Title = "Lesson 2: C++ Syntax",
						Description = "Let's break up the following code to understand it better",
						OrderNumerator = 2,
						OrderDenominator = 1,
						Units= new()
						{
							new Material()
							{
								Title = "Material 1",
								RequiredTime = TimeSpan.FromMinutes(15),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Content = "Answer question: .....?",
							 }
						}

					},
					   new Lesson()
					{
						LessonId = 3,
						Title = "Lesson 3: C++ OOP",
						Description = "OOP stands for Object-Oriented Programming.",
						OrderNumerator = 3,
						OrderDenominator = 1,
						Units= new()
						{
						   new Exam()
							{
								Title = "Exam 1",
								RequiredTime = TimeSpan.FromMinutes(1),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Questions=new()
								{
									new Question()
									{
									QuestionId = 1,
									Point = 2,
									ExamUnitId = 1,
									Content = " 1. How do you insert COMMENTS in C++ code?",
									Choices = new()
									{
										new Choice()
										{
											ChoiceId = 1,
											IsCorrect = true,
											Content = "a. // this is a comment"
										},
										new Choice()
										{
											ChoiceId = 2,
											IsCorrect = false,
											Content = "b. # this is a comment"
										}
									}

									},
									new Question()
									{
									QuestionId = 2,
									Point = 2,
									ExamUnitId = 1,
									Content = " 2. Which data type is used to create a variable that should store text?",
									Choices = new()
									{
										new Choice()
										{
											ChoiceId = 1,
											IsCorrect = true,
											Content = "a. String"
										},
										new Choice()
										{
											ChoiceId = 2,
											IsCorrect = false,
											Content = "b. Mystring"
										},
										new Choice()
										{
											ChoiceId = 3,
											IsCorrect = false,
											Content = "c. txt"
										}
									}

									}

								}
							}

						}

					}
				}
			},
			new Course()
			{
				CourseId = 3,
				Title = "Learn C",
				Description = "How to learn C in 10 hours",
				About = "I will teach...",
				Tier = CourseTier.Free,
				IsApproved = false,
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				TopicId = 1,
				PublisherUserId = 1,
				RatingAverage = 5,
				RatingTotal = 10,
				Lessons = new()
				{
						new Lesson()
						{
						LessonId = 1,
						Title = "Lesson 1: C Introduction",
						Description = "C is a general-purpose programming language created by Dennis Ritchie at the Bell Laboratories in 1972.\r\n\r\nIt is a very popular language, despite being old.\r\n\r\nC is strongly associated with UNIX, as it was developed to write the UNIX operating system.",
						OrderNumerator = 1,
						OrderDenominator = 1,
						Units= new()
						{
							new Material()
							{
								Title = "Material 2",
								RequiredTime = TimeSpan.FromMinutes(15),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Content =" dunno :D"
							}
						}
						}
				}
			},
			new Course()
			{
				CourseId = 4,
				Title = "Learn C#",
				Description = "How to learn C# in 10 hours",
				About = "I will teach...",
				Tier = CourseTier.Free,
				IsApproved = false,
				Created = DateTime.Now,
				LastModified = DateTime.Now,
				TopicId = 1,
				PublisherUserId = 1,
				RatingAverage = 5,
				RatingTotal = 15,
				Lessons = new()
				{
						new Lesson()
					   {
						LessonId = 1,
						Title = "Lesson 1: C# Introduction",
						Description = "C# is pronounced \"C-Sharp\".\r\n\r\nIt is an object-oriented programming language created by Microsoft that runs on the .NET Framework.\r\n\r\nC# has roots from the C family, and the language is close to other popular languages like C++ and Java.\r\n\r\nThe first version was released in year 2002. The latest version, C# 11, was released in November 2022.",
						OrderNumerator = 1,
						OrderDenominator = 1,
						 Units= new()
						{
							new Material()
							{
								Title = "Material 1",
								RequiredTime = TimeSpan.FromMinutes(15),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Content =" also dunno :D"
							}
						}
					   },
					  new Lesson()
					  {
						LessonId = 2,
						Title = "Lesson 2: C# Syntax",
						Description = "In the previous chapter, we created a C# file called Program.cs, and we used the following code to print \"Hello World\" to the screen",
						OrderNumerator = 2,
						OrderDenominator = 1,
						Units= new()
						{
							new Material()
							{
								Title = "Material 2",
								RequiredTime = TimeSpan.FromMinutes(15),
								OrderNumerator = 1 ,
								OrderDenominator = 1,
								Content =" also dunno :D"
							},
							new Exam()
							{
								Title = "Exam 1",
								RequiredTime = TimeSpan.FromMinutes(1),
								OrderNumerator = 2 ,
								OrderDenominator = 1,
								Questions=new()
								{
									new Question()
									{
									QuestionId = 1,
									Point = 2,
									ExamUnitId = 1,
									Content = " 1. How do you insert COMMENTS in C# code?",
									Choices = new()
									{
										new Choice()
										{
											ChoiceId = 1,
											IsCorrect = true,
											Content = "a. // this is a comment"
										},
										new Choice()
										{
											ChoiceId = 2,
											IsCorrect = false,
											Content = "b. # this is a comment"
										}
									}

									},
									new Question()
									{
									QuestionId = 2,
									Point = 2,
									ExamUnitId = 1,
									Content = " 2. Which data type is used to create a variable that should store text?",
									Choices = new()
									{
										new Choice()
										{
											ChoiceId = 1,
											IsCorrect = true,
											Content = "a. String"
										},
										new Choice()
										{
											ChoiceId = 2,
											IsCorrect = false,
											Content = "b. Mystring"
										},
										new Choice()
										{
											ChoiceId = 3,
											IsCorrect = false,
											Content = "c. txt"
										}
									}

									}

								}
							}

						}
					  }
				}
			}

		});
	};
}
