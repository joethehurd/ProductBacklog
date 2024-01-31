using Capstone.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Services
{   
    public static class DataAccessService
    {      
        static SQLiteConnection db;

        public static IList<User> userList;

        public static void Init()
        {
            try
            {
                // open db connection
                OpenDatabase();
               
                // create tables
                db.CreateTable<User>();
                db.CreateTable<Product>();
                db.CreateTable<Sprint>();
                db.CreateTable<Requirement>();

                // create users
                userList = GetAllUsers();
                if (!userList.Any())
                {
                    PopulateUsers();
                }                

                // close db connection
                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);                
            }         
        }    

        public static void OpenDatabase()
        {
            try
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "D424.db");
                db = new SQLiteConnection(databasePath);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }

        public static void CloseDatabase()
        {
            try
            {
                db.Close();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        #region Default Data

        public static void PopulateUsers()
        {
            try
            {
                OpenDatabase();

                // populate database with default users
                var query1 = new User
                {
                    Id = 1,
                    Username = "test",
                    Password = "test",
                    LoggedIn = "false"
                };
                var results1 = db.InsertOrReplace(query1);

                #region Admin

                var query2 = new User
                {
                    Id = 2,
                    Username = "admin",
                    Password = "protected",
                    LoggedIn = "false"
                };
                var results2 = db.InsertOrReplace(query2);
              
                var query3 = new Product
                {
                    Id = new Guid(),
                    Name = "Admin Product",
                    Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                    End = new DateTime(2024, 2, 16).ToUniversalTime(),
                    OwnerName = "Admin",
                    OwnerPhone = "111-111-1111",
                    OwnerEmail = "admin@capstone.demo",
                    Status = 2,
                    Priority = 1,
                    Alerts = "Off",
                    UserId = 2
                };
                var results3 = db.Insert(query3);

                var query4 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Admin Task",
                    Start = new DateTime(2024, 2, 12).ToUniversalTime(),
                    End = new DateTime(2024, 2, 16).ToUniversalTime(),
                    AssigneeName = "Admin",
                    AssigneePhone = "111-111-1111",
                    AssigneeEmail = "admin@capstone.demo",
                    Status = 1,
                    Priority = 1,
                    Alerts = "Off",
                    ProductId = query3.Id,
                    UserId = 2
                };
                var results4 = db.Insert(query4);

                var query5 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Admin Requirement",
                    Type = "Other",
                    Description = "Admin Description",
                    SprintId = query4.Id,
                    UserId = 2
                };
                var results5 = db.Insert(query5);

                #endregion

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }

        public static void PopulateTables()
        {               
            try
            {
                OpenDatabase();

                // populate database with default data                
                #region Design

                var query1 = new Product
                {
                    Id = new Guid(),
                    Name = "Design",
                    Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                    End = new DateTime(2024, 1, 5).ToUniversalTime(),
                    OwnerName = "Jane Doe",
                    OwnerPhone = "555-123-4567",
                    OwnerEmail = "jane.doe@capstone.demo",
                    Status = 3,
                    Priority = 2,
                    Alerts = "Off",
                    UserId = 1
                };
                var results1 = db.Insert(query1);

                var query2 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Wireframing",
                    Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                    End = new DateTime(2024, 1, 3).ToUniversalTime(),
                    AssigneeName = "John Smith",
                    AssigneePhone = "765-432-1555",
                    AssigneeEmail = "john.smith@capstone.demo",
                    Status = 3,
                    Priority = 3,
                    Alerts = "Off",
                    ProductId = query1.Id,
                    UserId = 1
                };
                var results2 = db.Insert(query2);

                var query3 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Diagramming",
                    Start = new DateTime(2024, 1, 3).ToUniversalTime(),
                    End = new DateTime(2024, 1, 5).ToUniversalTime(),
                    AssigneeName = "Sarah Williams",
                    AssigneePhone = "567-234-1444",
                    AssigneeEmail = "sarah.williams@capstone.demo",
                    Status = 3,
                    Priority = 1,
                    Alerts = "Off",
                    ProductId = query1.Id,
                    UserId = 1
                };
                var results3 = db.Insert(query3);

                var query4 = new Requirement
                {
                    Id = new Guid(),
                    Name = "High-Fidelity Wireframe",
                    Type = "Design",
                    Description = "Design a high-fidelity wireframe that fully illustrates the user interface",
                    SprintId = query2.Id,
                    UserId = 1
                };
                var results4 = db.Insert(query4);

                var query5 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Class & Database Diagrams",
                    Type = "Design",
                    Description = "Design class templates and relational database diagram",
                    SprintId = query3.Id,
                    UserId = 1
                };
                var results5 = db.Insert(query5);

                #endregion

                #region Development

                var query6 = new Product
                {
                    Id = new Guid(),
                    Name = "Development",
                    Start = new DateTime(2024, 1, 8).ToUniversalTime(),
                    End = new DateTime(2024, 2, 2).ToUniversalTime(),
                    OwnerName = "Chris Adams",
                    OwnerPhone = "325-183-7542",
                    OwnerEmail = "chris.adams@capstone.demo",
                    Status = 2,
                    Priority = 1,
                    Alerts = "Off",
                    UserId = 1
                };
                var results6 = db.Insert(query6);

                var query7 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Code Refactoring",
                    Start = new DateTime(2024, 1, 8).ToUniversalTime(),
                    End = new DateTime(2024, 1, 17).ToUniversalTime(),
                    AssigneeName = "Eric Ericson",
                    AssigneePhone = "489-472-1352",
                    AssigneeEmail = "eric.ericson@capstone.demo",
                    Status = 3,
                    Priority = 2,
                    Alerts = "Off",
                    ProductId = query6.Id,
                    UserId = 1
                };
                var results7 = db.Insert(query7);

                var query8 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Code Production",
                    Start = new DateTime(2024, 1, 22).ToUniversalTime(),
                    End = new DateTime(2024, 2, 2).ToUniversalTime(),
                    AssigneeName = "Eric Ericson",
                    AssigneePhone = "489-472-1352",
                    AssigneeEmail = "eric.ericson@capstone.demo",
                    Status = 2,
                    Priority = 1,
                    Alerts = "On",
                    ProductId = query6.Id,
                    UserId = 1
                };
                var results8 = db.Insert(query8);

                var query9 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Front-End Code",
                    Type = "Non-functional",
                    Description = "Refactor existing front-end code",
                    SprintId = query7.Id,
                    UserId = 1
                };
                var results9 = db.Insert(query9);

                var query10 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Back-End Code",
                    Type = "Functional",
                    Description = "Refactor existing back-end code",
                    SprintId = query7.Id,
                    UserId = 1
                };
                var results10 = db.Insert(query10);

                var query11 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Front-End Code",
                    Type = "Non-functional",
                    Description = "Produce new front-end code",
                    SprintId = query8.Id,
                    UserId = 1
                };
                var results11 = db.Insert(query11);

                var query12 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Back-End Code",
                    Type = "Functional",
                    Description = "Produce new back-end code",
                    SprintId = query8.Id,
                    UserId = 1
                };
                var results12 = db.Insert(query12);

                #endregion

                #region Testing

                var query13 = new Product
                {
                    Id = new Guid(),
                    Name = "Testing",
                    Start = new DateTime(2024, 1, 11).ToUniversalTime(),
                    End = new DateTime(2024, 2, 6).ToUniversalTime(),
                    OwnerName = "Jane Doe",
                    OwnerPhone = "555-123-4567",
                    OwnerEmail = "jane.doe@capstone.demo",
                    Status = 2,
                    Priority = 2,
                    Alerts = "Off",
                    UserId = 1
                };
                var results13 = db.Insert(query13);

                var query14 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Non-Functional Testing",
                    Start = new DateTime(2024, 1, 11).ToUniversalTime(),
                    End = new DateTime(2024, 1, 12).ToUniversalTime(),
                    AssigneeName = "John Smith",
                    AssigneePhone = "765-432-1555",
                    AssigneeEmail = "john.smith@capstone.demo",
                    Status = 3,
                    Priority = 3,
                    Alerts = "Off",
                    ProductId = query13.Id,
                    UserId = 1
                };
                var results14 = db.Insert(query14);

                var query15 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Functional Testing",
                    Start = new DateTime(2024, 1, 18).ToUniversalTime(),
                    End = new DateTime(2024, 1, 19).ToUniversalTime(),
                    AssigneeName = "John Smith",
                    AssigneePhone = "765-432-1555",
                    AssigneeEmail = "john.smith@capstone.demo",
                    Status = 1,
                    Priority = 2,
                    Alerts = "Off",
                    ProductId = query13.Id,
                    UserId = 1
                };
                var results15 = db.Insert(query15);

                var query16 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Unit Testing",
                    Start = new DateTime(2024, 2, 5).ToUniversalTime(),
                    End = new DateTime(2024, 2, 6).ToUniversalTime(),
                    AssigneeName = "Sarah Williams",
                    AssigneePhone = "567-234-1444",
                    AssigneeEmail = "sarah.williams@capstone.demo",
                    Status = 1,
                    Priority = 1,
                    Alerts = "Off",
                    ProductId = query13.Id,
                    UserId = 1
                };
                var results16 = db.Insert(query16);

                var query17 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Test Case",
                    Type = "Non-functional",
                    Description = "Ensure that the refactored front-end code performs as intended",
                    SprintId = query14.Id,
                    UserId = 1
                };
                var results17 = db.Insert(query17);

                var query18 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Test Case",
                    Type = "Functional",
                    Description = "Ensure that the refactored back-end code performs as intended",
                    SprintId = query15.Id,
                    UserId = 1
                };
                var results18 = db.Insert(query18);

                var query19 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Test Case",
                    Type = "System",
                    Description = "Ensure that the individual components perform as intended",
                    SprintId = query16.Id,
                    UserId = 1
                };
                var results19 = db.Insert(query19);

                #endregion

                #region Documentation            

                var query20 = new Product
                {
                    Id = new Guid(),
                    Name = "Documentation",
                    Start = new DateTime(2024, 2, 7).ToUniversalTime(),
                    End = new DateTime(2024, 2, 9).ToUniversalTime(),
                    OwnerName = "Jane Doe",
                    OwnerPhone = "555-123-4567",
                    OwnerEmail = "jane.doe@capstone.demo",
                    Status = 1,
                    Priority = 3,
                    Alerts = "Off",
                    UserId = 1
                };
                var results20 = db.Insert(query20);

                var query21 = new Sprint
                {
                    Id = new Guid(),
                    Name = "User Documentation",
                    Start = new DateTime(2024, 2, 7).ToUniversalTime(),
                    End = new DateTime(2024, 2, 9).ToUniversalTime(),
                    AssigneeName = "Eric Ericson",
                    AssigneePhone = "489-472-1352",
                    AssigneeEmail = "eric.ericson@capstone.demo",
                    Status = 1,
                    Priority = 3,
                    Alerts = "Off",
                    ProductId = query20.Id,
                    UserId = 1
                };
                var results21 = db.Insert(query21);

                var query22 = new Requirement
                {
                    Id = new Guid(),
                    Name = "User Guide",
                    Type = "User",
                    Description = "Create a user guide outlining the functionality, navigation, and operation of the application",
                    SprintId = query21.Id,
                    UserId = 1
                };
                var results22 = db.Insert(query22);

                #endregion

                #region Deployment           

                var query23 = new Product
                {
                    Id = new Guid(),
                    Name = "Deployment",
                    Start = new DateTime(2024, 2, 12).ToUniversalTime(),
                    End = new DateTime(2024, 2, 16).ToUniversalTime(),
                    OwnerName = "Chris Adams",
                    OwnerPhone = "325-183-7542",
                    OwnerEmail = "chris.adams@capstone.demo",
                    Status = 1,
                    Priority = 1,
                    Alerts = "Off",
                    UserId = 1
                };
                var results23 = db.Insert(query23);

                var query24 = new Sprint
                {
                    Id = new Guid(),
                    Name = "Deploy Application",
                    Start = new DateTime(2024, 2, 12).ToUniversalTime(),
                    End = new DateTime(2024, 2, 16).ToUniversalTime(),
                    AssigneeName = "Sarah Williams",
                    AssigneePhone = "567-234-1444",
                    AssigneeEmail = "sarah.williams@capstone.demo",
                    Status = 1,
                    Priority = 1,
                    Alerts = "Off",
                    ProductId = query23.Id,
                    UserId = 1
                };
                var results24 = db.Insert(query24);

                var query25 = new Requirement
                {
                    Id = new Guid(),
                    Name = "Completed Application",
                    Type = "Business",
                    Description = "Deploy the completed application to the marketplace",
                    SprintId = query24.Id,
                    UserId = 1
                };
                var results25 = db.Insert(query25);

                #endregion
                
                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }                     
        }
        
        #endregion

        #region Users

        public static List<User> GetAllUsers()
        {
            try
            {
                OpenDatabase();

                // get all users to populate user list on login page           
                string query = $"SELECT * FROM user";

                var results = db.Query<User>(query);

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static void UpdateUser(int id, string username, string password, string loggedIn)
        {
            try
            {
                OpenDatabase();

                // for updating logged-in status
                var query = new User
                {
                    Id = id,
                    Username = username,
                    Password = password,
                    LoggedIn = loggedIn
                };

                var results = db.Update(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        #endregion

        #region Products

        public static List<Product> GetAllProducts()
        {
            try
            {
                OpenDatabase();

                // get all products for testing          
                string query = $"SELECT * FROM product";

                var results = db.Query<Product>(query);

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }                  
        }
        public static List<Product> GetProductsByUser(int userId)
        {
            try
            {
                OpenDatabase();

                // to populate the active user's product summary list on the home page
                // get all products where product.userId == user.Id (userId)
                var results = db.Table<Product>().Where(t => t.UserId == userId).ToList();

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
        public static Product GetProduct(Guid productId)
        {
            try
            {
                OpenDatabase();

                // to populate the selected product's detailed view                           
                var results = db.Table<Product>().Where(t => t.Id == productId).ToList();

                CloseDatabase();

                return results[0];
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }         
        }
        public static void AddProduct(Guid productId, string name, DateTime start, DateTime end, string ownerName, string ownerPhone, string ownerEmail, int status, int priority, string alerts, int userId)
        {
            try
            {
                OpenDatabase();

                var query = new Product
                {
                    Id = productId,
                    Name = name,
                    Start = start.ToUniversalTime(),
                    End = end.ToUniversalTime(),
                    OwnerName = ownerName,
                    OwnerPhone = ownerPhone,
                    OwnerEmail = ownerEmail,
                    Status = status,
                    Priority = priority,
                    Alerts = alerts,
                    UserId = userId
                };

                var results = db.Insert(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static void DeleteProduct(Guid productId)
        {
            try
            {
                // get associated sprints where sprint.productId == (productId)            
                var sprints = GetSprintsByProduct(productId);

                // delete associated sprints           
                if (sprints.Any())
                {
                    foreach (Sprint sprint in sprints)
                    {
                        DeleteSprint(sprint.Id);
                    }
                }

                OpenDatabase();

                // delete product where product.id == (productId)      
                db.Delete<Product>(productId);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }
        public static void UpdateProduct(Guid productId, string name, DateTime start, DateTime end, string ownerName, string ownerPhone, string ownerEmail, int status, int priority, string alerts, int userId)
        {
            try
            {
                OpenDatabase();

                var query = new Product
                {
                    Id = productId,
                    Name = name,
                    Start = start.ToUniversalTime(),
                    End = end.ToUniversalTime(),
                    OwnerName = ownerName,
                    OwnerPhone = ownerPhone,
                    OwnerEmail = ownerEmail,
                    Status = status,
                    Priority = priority,
                    Alerts = alerts,
                    UserId = userId
                };

                var results = db.Update(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }

        #endregion

        #region Sprints
        public static List<Sprint> GetAllSprints()
        {
            try
            {
                OpenDatabase();

                // get all sprints           
                string query = $"SELECT * FROM sprint";

                var results = db.Query<Sprint>(query);

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }
        public static List<Sprint> GetSprintsByUser(int userId)
        {
            try
            {
                OpenDatabase();

                // to populate the active user's sprint summary list   
                // get all sprints where sprint.userId == user.Id (userId)
                var results = db.Table<Sprint>().Where(t => t.UserId == userId).ToList();

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
        public static List<Sprint> GetSprintsByProduct(Guid productId) 
        {
            try
            {
                OpenDatabase();

                // to populate the selected product's sprint summary list   
                // get all sprints where sprint.productId == product.Id (productId)
                var results = db.Table<Sprint>().Where(t => t.ProductId == productId).ToList();

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static Sprint GetSprint(Guid sprintId)
        {
            try
            {
                OpenDatabase();

                // to populate the selected sprint's detailed view   
                // get specific sprint where sprint.id == (sprintId)
                var results = db.Table<Sprint>().Where(t => t.Id == sprintId).ToList();

                CloseDatabase();

                return results[0];
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }
        public static void AddSprint(Guid sprintId, string name, DateTime start, DateTime end, string assigneeName, string assigneePhone, string assigneeEmail, int status, int priority, string alerts, Guid productId, int userId)
        {
            try
            {
                OpenDatabase();

                var query = new Sprint
                {
                    Id = sprintId,
                    Name = name,
                    Start = start.ToUniversalTime(),
                    End = end.ToUniversalTime(),
                    AssigneeName = assigneeName,
                    AssigneePhone = assigneePhone,
                    AssigneeEmail = assigneeEmail,
                    Status = status,
                    Priority = priority,
                    Alerts = alerts,
                    ProductId = productId,
                    UserId = userId
                };

                var results = db.Insert(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }
        public static void DeleteSprint(Guid sprintId)
        {
            try
            {
                // get associated requirements where requirement.sprintId == sprint.id
                var requirements = GetRequirementsBySprint(sprintId);

                // delete associated requirements
                if (requirements.Any())
                {
                    foreach (Requirement requirement in requirements)
                    {
                        DeleteRequirement(requirement.Id);
                    }
                }

                OpenDatabase();

                // delete sprint where sprint.id == (sprintId)
                db.Delete<Sprint>(sprintId);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static void UpdateSprint(Guid sprintId, string name, DateTime start, DateTime end, string assigneeName, string assigneePhone, string assigneeEmail, int status, int priority, string alerts, Guid productId, int userId)
        {
            try
            {
                OpenDatabase();

                var query = new Sprint
                {
                    Id = sprintId,
                    Name = name,
                    Start = start.ToUniversalTime(),
                    End = end.ToUniversalTime(),
                    AssigneeName = assigneeName,
                    AssigneePhone = assigneePhone,
                    AssigneeEmail = assigneeEmail,
                    Status = status,
                    Priority = priority,
                    Alerts = alerts,
                    ProductId = productId,
                    UserId = userId
                };

                var results = db.Update(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }

        #endregion

        #region Requirements

        public static List<Requirement> GetAllRequirements()
        {
            try
            {
                OpenDatabase();

                // get all requirements           
                string query = $"SELECT * FROM requirement";

                var results = db.Query<Requirement>(query);

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static List<Requirement> GetRequirementsByUser(int userId)
        {
            try
            {
                OpenDatabase();

                // to populate the active user's requirement summary list    
                // get all requirements where requirement.userId = user.Id (userId)
                var results = db.Table<Requirement>().Where(t => t.UserId == userId).ToList();

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
        public static List<Requirement> GetRequirementsBySprint(Guid sprintId) 
        {
            try
            {
                OpenDatabase();

                // to populate the selected sprint's requirement summary list    
                // get all requirements where requirement.sprintId = sprint.id (sprintId)
                var results = db.Table<Requirement>().Where(t => t.SprintId == sprintId).ToList();

                CloseDatabase();

                return results;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }          
        }
        public static Requirement GetRequirement(Guid requirementId)
        {
            try
            {
                OpenDatabase();

                // to populate the selected requirement's detailed view    
                // get specific requirement where requirement.id = (requirementId)
                var results = db.Table<Requirement>().Where(t => t.Id == requirementId).ToList();

                CloseDatabase();

                return results[0];
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static void AddRequirement(Guid requirementId, string name, string type, string description, Guid sprintId, int userId)
        {
            try
            {
                OpenDatabase();

                var query = new Requirement
                {
                    Id = requirementId,
                    Name = name,
                    Type = type,
                    Description = description,
                    SprintId = sprintId,
                    UserId = userId
                };

                var results = db.Insert(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }
        public static void DeleteRequirement(Guid requirementId)
        {
            try
            {
                OpenDatabase();

                // delete requirement where requirement.id == (requirementId)
                db.Delete<Requirement>(requirementId);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }           
        }
        public static void UpdateRequirement(Guid requirementId, string name, string type, string description, Guid sprintId, int userId)
        {
            try
            {
                OpenDatabase();

                var query = new Requirement
                {
                    Id = requirementId,
                    Name = name,
                    Type = type,
                    Description = description,
                    SprintId = sprintId,
                    UserId = userId
                };

                var results = db.Update(query);

                CloseDatabase();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }            
        }

        #endregion      
    }
}
