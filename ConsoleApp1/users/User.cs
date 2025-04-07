using StudyingTesting.poker_hands.game_area;
using System;
using System.Collections.Generic;

namespace StudyingTesting.users
{
    public class User
    {
        private string username;
        private List<User_Role> roles;
        private List<Table> tables;
        private bool havePermission;

        // Constructor to allow a blank user with no roles and tables
        public User()
        {
            // Initialize roles as null by default
            roles = new List<User_Role>();  // User starts without any roles
            tables = null;  // Tables are also null by default
        }

        public List<User_Role> Roles
        {
            get => roles;
            set
            {
                roles = value;
                // Only ADMIN or MANAGER can have tables
                if (roles != null && (roles.Contains(User_Role.ADMIN) || roles.Contains(User_Role.MANAGER)))
                {
                    tables = new List<Table>();  // Initialize tables if the user is ADMIN or MANAGER
                    havePermission = true;
                }
                else
                {
                    tables = null; // Set tables to null if no valid roles
                    havePermission = false;
                }
            }
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public void AddTable()
        {
            // If roles are null or empty, the user cannot add tables
            if (roles == null || roles.Count == 0)
            {
                throw new InvalidOperationException("User with no role should not be allowed to add a table.");
            }

            // Ensure the user has valid roles before adding a table
            if (tables == null)
            {
                throw new InvalidOperationException("This user is not allowed to add tables.");
            }

            // Ensure the user cannot have more than 3 tables
            if (tables.Count >= 3)
            {
                throw new InvalidOperationException("User cannot have more than 3 tables.");
            }

            // Add the table
            tables.Add(new Table());
        }

        public List<Table> GetTables()
        {
            return tables;
        }

        public bool GetPermission()
        {
            return havePermission;
        }
    }
}
