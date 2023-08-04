﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using NUnit.Framework;
using QuantConnect.Indicators;

namespace QuantConnect.Tests.Indicators
{
    [TestFixture]
    public class TimeSeriesForecastTests
    {   
        // Reference : https://tulipindicators.org/tsf
        [Test]
        public void TsfComputesCorrectly()
        {   
            // This data is from the reference website
            var tsf = new TimeSeriesForecast(5);
            var data = new[] {81.59m, 81.06m, 82.87m, 83.00m, 83.61m, 83.15m, 82.84m, 83.99m, 84.55m, 84.36m, 85.53m, 86.54m, 86.89m, 87.77m, 87.29m};
            var expectedOutput = new [] {0m, 0m, 0m, 0m, 84.22m, 84.21m, 83.12m, 83.68m, 84.44m, 85.02m, 85.98m, 86.82m, 87.63m, 88.67m, 88.23m};
            var reference = DateTime.MinValue;

            for (int i = 0; i < data.Length; i++)
            {
                
                tsf.Update(reference.AddDays(i+1),data[i]);
                Assert.AreEqual(expectedOutput[i],decimal.Round(tsf.Current.Value,2));

            }
        }

        [Test]
        public void IsReadyAfterPeriodUpdates()
        {
            var tsf = new TimeSeriesForecast(3);

            Assert.IsFalse(tsf.IsReady);
            tsf.Update(DateTime.UtcNow, 1m);
            Assert.IsFalse(tsf.IsReady);
            tsf.Update(DateTime.UtcNow, 1m);
            Assert.IsFalse(tsf.IsReady);
            tsf.Update(DateTime.UtcNow, 1m);
            Assert.IsTrue(tsf.IsReady);
        }

        [Test]
        public void ResetsProperly()
        {
            var tsf = new TimeSeriesForecast(3);
            tsf.Update(DateTime.Today, 1m);
            tsf.Update(DateTime.Today.AddSeconds(1), 2m);
            tsf.Update(DateTime.Today.AddSeconds(1), 1m);
            
            Assert.IsTrue(tsf.IsReady);
            
            tsf.Reset();

            TestHelper.AssertIndicatorIsInDefaultState(tsf);            
        }
    }
}