using System;
using System.Diagnostics;
using Xunit;

namespace Service.Integration.Tests
{
    public class RandomUserServiceTests
    {
        // get 1 user from the RandonUser API. A REST webservice. Less than 5 seconds

        [Fact]
        public void GetRandomUsers_Returns_Populated_User()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var randomUserService = new RandomUserService();

            var users = randomUserService.GetRandomUsersAsync(false, 1);

            stopWatch.Stop();

            Assert.Equal(1, users.Count);

            Console.Write(stopWatch.ElapsedMilliseconds);

            Assert.True(stopWatch.ElapsedMilliseconds < 5000);
        }

        // get 50 users

        [Fact]
        public void GetRandomUsers_Populate_Bulk_Data_Multiple_Calls_Async()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var randomUserService = new RandomUserService();

            var users = randomUserService.GetRandomUsersAsync(false, 50);

            stopWatch.Stop();

            Assert.Equal(50, users.Count);
            foreach (var user in users)
            {
                Assert.NotNull(user.registered);
                Assert.False(string.IsNullOrEmpty(user.name.first));
            }

            Console.Write(stopWatch.ElapsedMilliseconds);

            Assert.True(stopWatch.ElapsedMilliseconds < 80000);
        }

        // how long would it take calling the service once with result=50?

        [Fact]
        public void GetRandomUsers_Populate_Bulk_Data_Single_Call_Async()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var randomUserService = new RandomUserService();

            var users = randomUserService.GetRandomUsersAsync(true, 50);

            stopWatch.Stop();

            Assert.Equal(50, users.Count);
            foreach (var user in users)
            {
                Assert.NotNull(user.registered);
                Assert.False(string.IsNullOrEmpty(user.name.first));
            }

            Console.Write(stopWatch.ElapsedMilliseconds);

            Assert.True(stopWatch.ElapsedMilliseconds < 10000);
        }

        // how long would it take without async?

        // how long with a seed?

        // create output - a list of users along with images
    }
}
