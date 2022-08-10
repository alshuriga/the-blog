USE [MiniBlog.Identity]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'70c6c9a4-80a4-457a-a623-1d1a5a39ddb9', N'Admins', N'ADMINS', N'0d9c81fc-1a81-4d9d-930f-ba0c4b6eca09')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'12c43fe4-37cb-425b-b3b6-7bfef8222c24', N'admin', N'ADMIN', N'admin@example.com', N'ADMIN@EXAMPLE.COM', 0, N'AQAAAAEAACcQAAAAEOLU1Q/0QeHA35EfkilzUycej3W/p8kiUkjRIx+XaovGhF646rYMtxPLpuHeGJSXdQ==', N'LD65SGMN2FECOFFBRY3AC5YTB54ZON3V', N'b3ea9b1f-64a9-4ba2-85d1-04eb2239f506', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'161a12ec-c857-4fa6-85b3-7891b71418e8', N'normal', N'NORMAL', N'normal@basic.com', N'NORMAL@BASIC.COM', 0, N'AQAAAAEAACcQAAAAECAQwzOqrB9rYmzcwi4ZTdkJkWQVtoksmo5QnbNHyWjIPTI1a7i/6dYTqq4Udfb5yQ==', N'NPPOBB3F7EVP5U76X5FYW4XYWZ2PT6VM', N'efa1f50f-80c4-4736-8a7b-b5395bf97a03', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'12c43fe4-37cb-425b-b3b6-7bfef8222c24', N'70c6c9a4-80a4-457a-a623-1d1a5a39ddb9')
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20220806161723_Initial', N'6.0.7')
GO
