using BlazorWithRedux.Store.Counter.Reducers;
using BlazorWithRedux.Store.Counter.Actions;
using BlazorWithRedux.Store.Counter.State;
using Fluxor.Undo;
using FluentAssertions;
using UnitTestsForBlazorWithRedux.utils;

namespace UnitTestsForBlazorWithRedux
{
        public class CounterActionsTests
        {
            [Fact]
            public void TestCounterActionsDirectlyOnReducers()
            {

                var state = new UndoableCounterState();
                state.CreateInitialState(); //use the extension method to create the initial state

                var reducer = new CounterReducers();

                var actions = new object[]
                {
                    new AddCounter(),
                    new AddCounter(),
                    new AddCounter(),
                    new SubCounter(),
                    new UndoAction<UndoableCounterState>()
                };

                foreach (var action in actions)
                {
                    state = reducer.Reduce(state, action);
                }

                state.Should().BeEquivalentTo(
                    new UndoableCounterState
                    {
                        Past = new[]
                        {
                            new CounterState { Count = 0 },
                            new CounterState { Count = 1 },
                            new CounterState { Count = 2 },
                        },
                        Present = new CounterState { Count = 3 },
                        Future = new[]
                        {
                            new CounterState { Count = 2 },
                        },
                    });
            }

            [Fact]
            public void TestCounterActionsWithFeatureExtension()
            {

                var state = new UndoableCounterState();
                state.CreateInitialState(); 
                //use the extension method to create the initial state

                state = state
                            .WithCounter(1)
                            .WithCounter(1)
                            .WithCounter(1)
                            .WithCounter(-1)
                            .WithUndoOne();
                
                state.Should().BeEquivalentTo(
                new UndoableCounterState
                {
                    Past = new[]
                    {
                    new CounterState { Count = 0 },
                    new CounterState { Count = 1 },
                    new CounterState { Count = 2 },
                    },
                    Present = new CounterState { Count = 3 },
                    Future = new[]
                    {
                    new CounterState { Count = 2 },
                    },
                });
            }

            [Fact]
            public void TestAllUndoableActionsOnCounter()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState(); //use the extension method to create the initial state

                // Apply actions
                state = state
                    .WithCounter(1)
                    .WithCounter(1)
                    .WithCounter(1)
                    .WithCounter(-1)
                    .WithUndoOne()
                    .WithRedoOne()
                    .WithUndoAll()
                    .WithRedoAll()
                    .WithJump(-1) //jump to the past
                    .WithJump(1); //jump to the future

                // Assert the expected state change
                state.Should().BeEquivalentTo(
                    new UndoableCounterState
                    {
                        Past = new[]
                        {
                            new CounterState { Count = 0 },
                            new CounterState { Count = 1 },
                            new CounterState { Count = 2 },
                            new CounterState { Count = 3 }
                        },
                        Present = new CounterState { Count = 2 },
                    });
            }

            [Fact]
            public void TestAddCounterAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1);

                state.Present.Count.Should().Be(1);
            }

            [Fact]
            public void TestSubCounterAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(-1);

                state.Present.Count.Should().Be(-1);
            }

            [Fact]
            public void TestUndoOneAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithUndoOne();

                state.Present.Count.Should().Be(0);
            }

            [Fact]
            public void TestRedoOneAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithUndoOne().WithRedoOne();

                state.Present.Count.Should().Be(1);
            }

            [Fact]
            public void TestUndoAllAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithCounter(1).WithUndoAll();

                state.Present.Count.Should().Be(0);
            }

            [Fact]
            public void TestRedoAllAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithCounter(1).WithUndoAll().WithRedoAll();

                state.Present.Count.Should().Be(2);
            }

            [Fact]
            public void TestJumpToPastAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithCounter(1).WithJump(-1);

                state.Present.Count.Should().Be(1);
            }

            [Fact]
            public void TestJumpToFutureAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithCounter(1).WithJump(-1).WithJump(1);

                state.Present.Count.Should().Be(2);
            }

            [Fact]
            public void TestPerformActionAfterUndoOneAction()
            {
                var state = new UndoableCounterState();
                state.CreateInitialState();

                state = state.WithCounter(1).WithCounter(1).WithCounter(1).WithCounter(-1).WithUndoOne().WithCounter(1);

                state.Should().BeEquivalentTo(
                    new UndoableCounterState
                    {
                        Past = new[]
                        {
                            new CounterState { Count = 0 },
                            new CounterState { Count = 1 },
                            new CounterState { Count = 2 },
                            new CounterState { Count = 3 }
                        },
                        Present = new CounterState { Count = 4 }
                    });
        }   

    }
}
