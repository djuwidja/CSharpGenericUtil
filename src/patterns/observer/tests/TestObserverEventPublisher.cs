using System.Collections.Generic;
using NUnit.Framework;
using Djuwidja.GenericUtil.Patterns.Observer;

namespace Djuwidja.GenericUtil.Patterns.Observer.Tests
{
    public class StateChangeEventPublisherTestSuite
    {
        [Test]
        public void CanInitialize()
        {
            ObserverEventPublisher<int, ObserverEvent<int>> testObj = new ObserverEventPublisher<int, ObserverEvent<int>>();
            Assert.IsNotNull(testObj);
        }

        [Test]
        public void CanObservePropertyChanges()
        {
            int numStrDexEventReceived = 0;

            TestState testObj = new TestState();
            // This subscriber only concerns itself with changes in STR and DEX.
            TestState.EventSubscriberCB strDexCb = (List<TestStateChangeEvent> changeEventList) =>
            {
                Assert.AreEqual(2, changeEventList.Count);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.STR_EVT, 1);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.DEX_EVT, 1);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.INT_EVT, 0);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.WIS_EVT, 0);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.CON_EVT, 0);

                Assert.AreEqual(10, testObj.Strength);
                Assert.AreEqual(5, testObj.Dexterity);

                numStrDexEventReceived++;
            };

            testObj.Subscribe(strDexCb, TestStateChangeEvent.STR_EVT, TestStateChangeEvent.DEX_EVT);

            // This subscriber only conerns itself with changes in INT.
            int numIntEventReceived = 0;
            TestState.EventSubscriberCB intCb = (List<TestStateChangeEvent> changeEventList) =>
            {
                Assert.AreEqual(2, changeEventList.Count);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.STR_EVT, 0);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.DEX_EVT, 0);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.INT_EVT, 2);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.WIS_EVT, 0);
                AssertIsContainsEvent(changeEventList, TestStateChangeEvent.CON_EVT, 0);

                // only stable state is observed.
                Assert.AreEqual(10, testObj.Intelligence);

                numIntEventReceived++;
            };

            testObj.Subscribe(intCb, TestStateChangeEvent.INT_EVT);

            // This subscriber only concerns intself with changes in CON.
            int numConEventReceived = 0;
            TestState.EventSubscriberCB conCb = (List<TestStateChangeEvent> changeEventList) =>
            {
                numConEventReceived++;
            };
            testObj.Subscribe(conCb, TestStateChangeEvent.CON_EVT);

            // make the changes
            testObj.Strength = 10;
            testObj.Dexterity = 5;
            testObj.Intelligence = 8;
            testObj.Intelligence += 2;

            Assert.IsTrue(testObj.HasUnpublishedChanges);
            testObj.Publish();

            // when a subscriber subscribes to more than 1 propertyAlias and both of them were triggered, only 1 event was received.
            Assert.AreEqual(1, numStrDexEventReceived);
            Assert.AreEqual(1, numIntEventReceived);
            // when a subscriber subscribes to a propertyAlias that was not triggered, no event was received.
            Assert.AreEqual(0, numConEventReceived);
            // All unpublished changes should be published.
            Assert.IsFalse(testObj.HasUnpublishedChanges);
        }

        private void AssertIsContainsEvent(List<TestStateChangeEvent> changeEventList, int evtType, int evtCount)
        {
            int numEventInList = 0;
            foreach (TestStateChangeEvent changeEvent in changeEventList)
            {
                if (changeEvent.EventType == evtType)
                {
                    numEventInList++;
                }
            }

            Assert.AreEqual(evtCount, numEventInList);
        }

        [Test]
        public void CanSubscribeAndUnsubscribeObserver()
        {
            TestState testObj = new TestState();
            TestState.EventSubscriberCB cb1 = (List<TestStateChangeEvent> changeEventList) =>
            {

            };

            TestState.EventSubscriberCB cb2 = (List<TestStateChangeEvent> changeEventList) =>
            {

            };

            // case 1 register
            testObj.Subscribe(cb1, TestStateChangeEvent.INT_EVT);
            testObj.Subscribe(cb2, TestStateChangeEvent.CON_EVT);
            Assert.True(testObj.IsObservingEventID(TestStateChangeEvent.INT_EVT));
            Assert.True(testObj.IsSubscribedCB(cb1));
            Assert.True(testObj.IsObservingEventID(TestStateChangeEvent.CON_EVT));
            Assert.True(testObj.IsSubscribedCB(cb2));

            // case 2 unregister alias with wrong cb
            testObj.Unsubscribe(cb2, TestStateChangeEvent.INT_EVT);
            Assert.True(testObj.IsObservingEventID(TestStateChangeEvent.INT_EVT));
            Assert.True(testObj.IsSubscribedCB(cb1));

            // case 3 unregister alias with wrong alias
            testObj.Unsubscribe(cb1, TestStateChangeEvent.WIS_EVT);
            Assert.True(testObj.IsObservingEventID(TestStateChangeEvent.INT_EVT));
            Assert.True(testObj.IsSubscribedCB(cb1));

            // case 4 unregister successfully
            testObj.Unsubscribe(cb1, TestStateChangeEvent.INT_EVT);
            Assert.False(testObj.IsObservingEventID(TestStateChangeEvent.INT_EVT));
            Assert.False(testObj.IsSubscribedCB(cb1));
        }
    }

    class TestState : ObserverEventPublisher<int, TestStateChangeEvent>
    {
        private int _strength = 0;
        private int _dexterity = 0;
        private int _intelligence = 0;
        private int _wisdom = 0;
        private int _constitution = 0;

        public TestState() : base()
        {

        }

        public int Strength
        {
            get => _strength;
            set
            {
                var strength = _strength;
                _strength = value;
                if (_strength != strength)
                {
                    this.RegisterChange(new StrChangeEvent(_strength - strength));
                }
            }
        }

        public int Dexterity
        {
            get => _dexterity;
            set
            {
                var dexterity = _dexterity;
                _dexterity = value;
                if (_dexterity != dexterity)
                {
                    this.RegisterChange(new DexChangeEvent(_dexterity - dexterity));
                }
            }
        }

        public int Intelligence
        {
            get => _intelligence;
            set
            {
                var intelligence = _intelligence;
                _intelligence = value;
                if (_intelligence != intelligence)
                {
                    this.RegisterChange(new IntChangeEvent(_intelligence - intelligence));
                }
            }
        }

        public int Wisdom
        {
            get => _wisdom;
            set
            {
                var wisdom = _wisdom;
                _wisdom = value;
                if (_wisdom != wisdom)
                {
                    this.RegisterChange(new WisChangeEvent(_wisdom - wisdom));
                }
            }
        }

        public int Constitution
        {
            get => _constitution;
            set
            {
                var constitution = _constitution;
                _constitution = value;
                if (_constitution != constitution)
                {
                    this.RegisterChange(new ConChangeEvent(_constitution - constitution));
                }
            }
        }
    }

    class TestStateChangeEvent : ObserverEvent<int>
    {
        public const int STR_EVT = 0;
        public const int DEX_EVT = 1;
        public const int INT_EVT = 2;
        public const int WIS_EVT = 3;
        public const int CON_EVT = 4;

        public TestStateChangeEvent(int evtType) : base(evtType)
        {

        }
    }

    class AbilityScoreChangeEvent : TestStateChangeEvent
    {
        private int _change;

        public AbilityScoreChangeEvent(int evtType, int change) : base(evtType)
        {
            _change = change;
        }

        public int Change
        {
            get => _change;
        }
    }

    class StrChangeEvent : AbilityScoreChangeEvent
    {
        public StrChangeEvent(int change) : base(STR_EVT, change)
        {

        }
    }

    class DexChangeEvent : AbilityScoreChangeEvent
    {
        public DexChangeEvent(int change) : base(DEX_EVT, change)
        {

        }
    }

    class IntChangeEvent : AbilityScoreChangeEvent
    {
        public IntChangeEvent(int change) : base(INT_EVT, change)
        {

        }
    }

    class WisChangeEvent : AbilityScoreChangeEvent
    {
        public WisChangeEvent(int change) : base(WIS_EVT, change)
        {

        }
    }

    class ConChangeEvent : AbilityScoreChangeEvent
    {
        public ConChangeEvent(int change) : base(CON_EVT, change)
        {

        }
    }


}