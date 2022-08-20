using MiniBlog.Core.Entities;

namespace MiniBlog.Tests.UnitTests.Mocks
{
    public static class SeedTestData
    {
        public static IEnumerable<Post> Posts(int number)
        {
            Post[] posts = new Post[number];
            for (int i = 0; i < posts.Length; i++)
            {
                posts[i] = new Post
                {
                    Id = i,
                    Header = $"Post_{i}",
                    Text = $"Post number {i}: lorem ipsum",
                    DateTime = DateTime.MinValue + TimeSpan.FromMinutes(i),
                    Commentaries = Commentaries(10, i).Where(p => p.PostId == i).ToList(),
                    Tags = Tags(5).ToList()
                };
                
            }
            return posts;
        }
        public static IEnumerable<Tag> Tags(int number)
        {
            Tag[] tags = new Tag[number];
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = new Tag
                {
                    Id = i,
                    Name = $"testtag{i}"
                };
            }
            return tags;
        }

        public static IEnumerable<Commentary> Commentaries(int number, long? postid = null)
        {
            var commentaries = new Commentary[number];
                for (int i = 0; i < 10; i++)
                {

                commentaries[i] = new Commentary
                        {
                            Email = $"random_{i}@email.com",
                            Text = $"lorem ipsum lorem ipsum lorem ipsum",
                            DateTime = DateTime.MinValue + TimeSpan.FromMinutes(i),
                            Username = $"User_{i}",
                            PostId = postid.HasValue ? (long)i : postid!.Value,
                        };
                }
            return commentaries;
        }

    }
}
