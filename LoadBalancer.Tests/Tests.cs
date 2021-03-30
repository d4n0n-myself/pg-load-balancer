using NUnit.Framework;

namespace LoadBalancer.Tests
{
    public class Tests
    {
        #region TestSuccess
        [Test]
        public void ReturnSuccess()
        {
        }
        [Test]
        public void EnqueueSuccess()
        {
        }

        #endregion

        #region Domain
        [Test]
        public void ChooseOnlineServer()
        {
        }
        [Test]
        public void ChooseAvailableServer()
        {
        }

        #endregion

        #region postgres error

        /// <summary>
        /// Вернуть ошибку postgres
        /// </summary>
        [Test]
        public void ReturnErrorIfPostgresError()
        {
        }

        /// <summary>
        /// Поставить в response storage ошибку postgres
        /// </summary>
        [Test]
        public void EnqueueErrorIfPostgresError()
        {
        }

        #endregion

        #region MaxSessions parameter

        [Test]
        public void DenyIfTooManyConnects()
        {
            // check by parameter
        }

        [Test]
        public void EnqueueIfTooManyConnects()
        {
        }

        #endregion
    }
}