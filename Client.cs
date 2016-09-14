
using System;
using System.Threading;
namespace TruLiveEncoder.WD
{
    /// <summary>
    /// Client for WD
    /// </summary>
    public class Client : IDisposable
    {
        IAuthentication _iath = null;
        Thread _thconnection = null;
        pwd_empmgmEntities1 pwd = null;
        #region IDisposable Implementation
        bool _isdispose = false;
        protected virtual void Dispose(bool _isdispose)
        {
            if (!this._isdispose && _isdispose)
            {
                this._isdispose = true;
                if (_onAuthentication != null)
                    foreach (Delegate d in _onAuthentication.GetInvocationList())
                        OnAuthentication -= (ResponseEventHandle)d;

                if (_thconnection != null)
                {
                    while (_thconnection.IsAlive)
                    {
                        _thconnection.Abort();
                        Thread.Sleep(100);
                    }
                }
                Disconnect();
            }
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        /// <summary>
        /// Constructor of client
        /// </summary>
        public Client()
        {
            pwd = new pwd_empmgmEntities1();
        }

        /// <summary>
        /// Connect to application
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="password"></param>
        public void Connect(string _userName, string password)
        {
           
            try
            {
                var v = pwd.pwd_employee.SqlQuery("select * from pwd_employee where username=@uname", _userName);
                if (v == null)
                    throw new Exception("Invalid username or password!!!");
                var res=v.GetEnumerator();
                if(!res.MoveNext())
                    throw new Exception("Invalid username or password!!!");
                if(res.Current.id>3 && res.Current.division>0)
                    throw new Exception(string.Format("Retry to login after {0}" ,res.Current.division));
               
              
                /// Attempt count reset
                if (string.Compare(res.Current.name, "", true) == 0)
                {
                    /// Attempt count reset
                    //res.Current.id == 0;
                    OnAuthenticationEventRaised(new ResponseEventArgs(new Authentication(new User(res.Current.name, null))));
                }
                else
                {
                    //Increase Attempt count
                    //res.Current.id += 1;
                    throw new Exception("Invalid username or password!!!");
                }
                pwd.SaveChanges();

            }
            catch (Exception ex)
            {
                OnAuthenticationEventRaised(new ResponseEventArgs(ex));
            }
            
        }

        /*   using (var dbContextTransaction = context.Database.BeginTransaction())
                  {
                      try
                      {
                          context.Database.ExecuteSqlCommand(
                              @"UPDATE Blogs SET Rating = 5" +
                                  " WHERE Name LIKE '%Entity Framework%'"
                              );

                          var query = context.Posts.Where(p => p.Blog.Rating >= 5);
                          foreach (var post in query)
                          {
                              post.Title += "[Cool Blog]";
                          }

                          context.SaveChanges();

                          dbContextTransaction.Commit();
                      }
                      catch (Exception)
                      {
                          dbContextTransaction.Rollback();
                      }
                  }
         * 
          class TransactionSample
      {
          public static void EnlistTransaction()
          {
              int retries = 3;
              string queueName = @".\Fulfilling";

              // Define variables that we need to add an item.
              short quantity = 2;
              int productId = 750;
              int orderId = 43680;

              // Define a long-running object context.
              AdventureWorksEntities context
                  = new AdventureWorksEntities();

              bool success = false;

              // Wrap the operation in a retry loop.
              for (int i = 0; i < retries; i++)
              {
                  // Define a transaction scope for the operations.
                  using (TransactionScope transaction = new TransactionScope())
                  {
                      try
                      {
                          // Define a query that returns a order by order ID.
                          SalesOrderHeader order =
                          context.SalesOrderHeaders.Where
                          ("it.SalesOrderID = @id", new ObjectParameter(
                           "id", orderId)).First();

                          // Load items for the order, if not already loaded.
                          if (!order.SalesOrderDetails.IsLoaded)
                          {
                              order.SalesOrderDetails.Load();
                          }

                          // Load the customer, if not already loaded.
                          if (!order.ContactReference.IsLoaded)
                          {
                              order.ContactReference.Load();
                          }

                          // Create a new item for an existing order.
                          SalesOrderDetail newItem = SalesOrderDetail.CreateSalesOrderDetail(
                              0, 0, quantity, productId, 1, 0, 0, 0, Guid.NewGuid(), DateTime.Today);

                          // Add new item to the order.
                          order.SalesOrderDetails.Add(newItem);

                          // Save changes pessimistically. This means that changes 
                          // must be accepted manually once the transaction succeeds.
                          context.SaveChanges(SaveOptions.DetectChangesBeforeSave);

                          // Create the message queue if it does not already exist.
                          if (!MessageQueue.Exists(queueName))
                          {
                              MessageQueue.Create(queueName);
                          }

                          // Initiate fulfilling order by sending a message.
                          using (MessageQueue q = new MessageQueue(queueName))
                          {
                              System.Messaging.Message msg =
                                  new System.Messaging.Message(String.Format(
                                      "<order customerId='{0}'>" +
                                      "<orderLine product='{1}' quantity='{2}' />" +
                                      "</order>", order.Contact.ContactID,
                                  newItem.ProductID, newItem.OrderQty));

                              // Send the message to the queue.
                              q.Send(msg);
                          }

                          // Mark the transaction as complete.
                          transaction.Complete();
                          success = true;
                          break;
                      }
                      catch (Exception ex)
                      {
                          // Handle errors and deadlocks here and retry if needed.
                          // Allow an UpdateException to pass through and 
                          // retry, otherwise stop the execution.
                          if (ex.GetType() != typeof(UpdateException))
                          {
                              Console.WriteLine("An error occured. "
                                  + "The operation cannot be retried."
                                  + ex.Message);
                              break;
                          }
                          // If we get to this point, the operation will be retried.
                      }
                  }
              }
              if (success)
              {
                  // Reset the context since the operation succeeded.
                  context.AcceptAllChanges();
              }
              else
              {
                  Console.WriteLine("The operation could not be completed in "
                      + retries + " tries.");
              }

              // Dispose the object context.
              context.Dispose();
          }
      }
       
       
       
       
         */
        /// <summary>
        /// Disconnect application
        /// </summary>
        public void Disconnect()
        {
            if (_iath != null)
                _iath.Dispose();
            _iath = null;
        }


        /// <summary>
        /// Event lock object 
        /// </summary>
        object _authenticationEventlock = new object();
        /// <summary>
        /// Event of server response
        /// </summary>
        event ResponseEventHandle _onAuthentication;


        /// <summary>
        /// Event of server response
        /// </summary>
        public event ResponseEventHandle OnAuthentication
        {
            add
            {
                lock (_authenticationEventlock)
                {
                    _onAuthentication += value;
                }
            }
            remove
            {
                lock (_authenticationEventlock)
                {
                    _onAuthentication -= value;
                }
            }
        }

        /// <summary>
        /// Raised buffer progress event
        /// </summary>
        /// <param name="value"></param>
        /// <param name="_isInternet"></param>
        public void OnAuthenticationEventRaised(ResponseEventArgs e)
        {
            ResponseEventHandle handler;
            lock (_authenticationEventlock)
            {
                handler = _onAuthentication;
            }
            if (handler != null)
                handler(this, e);
        }
    }
}
