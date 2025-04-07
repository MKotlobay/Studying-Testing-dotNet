using StudyingTesting.poker_hands;
using StudyingTesting.users;
using StudyingTesting.poker_hands.game_area;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestMyProject
{
    [TestClass]
    public class ProcessTest
    {
        // First Test: Checks if Admin and Manager can add tables but Player cannot
        [TestMethod]
        public void TestAddTableByRole()
        {
            // Create a list of users (Admin, Manager, Player)
            User[] users = new User[]
            {
                new User { Roles = new List<User_Role> { User_Role.ADMIN } },
                new User { Roles = new List<User_Role> { User_Role.MANAGER } },
                new User { Roles = new List<User_Role> { User_Role.PLAYER } }
            };

            foreach (var user in users)
            {
                // Try to add a table for each user
                if (user.Roles.Contains(User_Role.ADMIN) || user.Roles.Contains(User_Role.MANAGER))
                {
                    user.AddTable();
                    // Admin and Manager should be able to add a table
                    Assert.AreEqual(1, user.GetTables().Count, $"{user.Roles[0]} should be able to add a table.");
                }
                else if (user.Roles.Contains(User_Role.PLAYER) && user.GetTables() != null)
                {
                    // Player should NOT be able to add a table
                    Assert.ThrowsException<System.InvalidOperationException>(() => user.AddTable(),
                        "Player should not be able to add a table.");
                }
            }
        }
        
        // Second Test: Checks limit for tables
        [TestMethod]
        public void TestAddTableLimit()
        {
            User[] users = new User[]
            {
                new User { Roles = new List<User_Role> { User_Role.ADMIN } },
                new User { Roles = new List<User_Role> { User_Role.MANAGER } },
                new User { Roles = new List<User_Role> { User_Role.PLAYER } }
            };

            foreach (var user in users)
            {
                if (user.Roles.Contains(User_Role.ADMIN) || user.Roles.Contains(User_Role.MANAGER))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        user.AddTable();
                    }

                    // 4th should throw
                    Assert.ThrowsException<InvalidOperationException>(() => user.AddTable(),
                        $"{user.Roles[0]} should not be able to add more than 3 tables.");
                }
                else if (user.Roles.Contains(User_Role.PLAYER))
                {
                    Assert.ThrowsException<InvalidOperationException>(() => user.AddTable(),
                        "Player should not be allowed to add any table.");
                }
            }
        }

        
        // Third Test: Test Permissions
        [TestMethod]
        public void TestPermissions()
        {
            var method = typeof(User).GetMethod("GetPermission");
            if (method == null)
            {
                Assert.Fail("Missing method: User.GetPermission(). Please implement it before running this test.");
            }

            User[] users = new User[]
            {
                new User { Roles = new List<User_Role> { User_Role.ADMIN } },
                new User { Roles = new List<User_Role> { User_Role.MANAGER } },
                new User { Roles = new List<User_Role> { User_Role.PLAYER } }
            };

            foreach (var user in users)
            {
                bool hasPermission = (bool)method.Invoke(user, null);

                if (user.Roles.Contains(User_Role.ADMIN) || user.Roles.Contains(User_Role.MANAGER))
                {
                    Assert.IsTrue(hasPermission, $"{user.Roles[0]} should have permission to add tables.");
                }
                else if (user.Roles.Contains(User_Role.PLAYER))
                {
                    Assert.IsFalse(hasPermission, "Player should not have permission to add tables.");
                }
            }
        }
    }
}
