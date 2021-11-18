namespace UnitTest;

public class CoordinateTrackerTest {
    #region SetCoordinate

    [Fact]
    public void SetCoordinate_MouseEventArgs_Sets_XY_With_ClientXY() {
        const double X = 10;
        const double Y = 20;

        CoordinateTracker coordinateTracker = new();

        coordinateTracker.SetCoordinate(new MouseEventArgs() {
            ClientX = X,
            ClientY = Y
        });
        Assert.Equal((X, Y), (coordinateTracker.x, coordinateTracker.y));
    }

    [Fact]
    public void SetCoordinate_TouchEventArgs_Sets_XY_With_ClientXY_Of_First_TargetTouchesEntry() {
        const double X = 10;
        const double Y = 20;

        CoordinateTracker coordinateTracker = new();

        coordinateTracker.SetCoordinate(new TouchEventArgs() {
            TargetTouches = new TouchPoint[1] {
                new TouchPoint() {
                    ClientX = X,
                    ClientY = Y
                }
            }
        });
        Assert.Equal((X, Y), (coordinateTracker.x, coordinateTracker.y));
    }

    [Fact]
    public void SetCoordinate_EventArgs_Works_Only_With_Mouse_Or_Touch() {
        const double X = 10;
        const double Y = 20;

        // MouseEventArgs
        {
            CoordinateTracker coordinateTracker = new();

            EventArgs e = new MouseEventArgs() {
                ClientX = X,
                ClientY = Y
            };

            coordinateTracker.SetCoordinate(e);
            Assert.Equal((X, Y), (coordinateTracker.x, coordinateTracker.y));
        }

        // TouchEventArgs
        {
            CoordinateTracker coordinateTracker = new();

            EventArgs e = new TouchEventArgs() {
                TargetTouches = new TouchPoint[1] {
                    new TouchPoint() {
                        ClientX = X,
                        ClientY = Y
                    }
                }
            };

            coordinateTracker.SetCoordinate(e);
            Assert.Equal((X, Y), (coordinateTracker.x, coordinateTracker.y));
        }

        // None
        {
            CoordinateTracker coordinateTracker = new();

            EventArgs e = new();

            Assert.Throws<ArgumentException>(() => coordinateTracker.SetCoordinate(e));
        }
    }

    #endregion


    #region MoveCoordinate

    [Fact]
    public void MoveCoordinate_MouseEventArgs_Moves_XY_With_ClientXY() {
        const double START_X = 100;
        const double START_Y = 200;
        const double MOVE_X = 10;
        const double MOVE_Y = 20;

        CoordinateTracker coordinateTracker = new();

        coordinateTracker.SetCoordinate(new MouseEventArgs() {
            ClientX = START_X,
            ClientY = START_Y
        });

        (double dx, double dy) = coordinateTracker.MoveCoordinate(new MouseEventArgs() {
            ClientX = MOVE_X,
            ClientY = MOVE_Y
        });
        Assert.Equal((MOVE_X - START_X, MOVE_Y - START_Y), (dx, dy));

        Assert.Equal((MOVE_X, MOVE_Y), (coordinateTracker.x, coordinateTracker.y));
    }

    [Fact]
    public void MoveCoordinate_TouchEventArgs_Moves_XY_With_ClientXY_Of_First_TargetTouchesEntry() {
        const double START_X = 100;
        const double START_Y = 200;
        const double MOVE_X = 10;
        const double MOVE_Y = 20;

        CoordinateTracker coordinateTracker = new();

        coordinateTracker.SetCoordinate(new TouchEventArgs() {
            TargetTouches = new TouchPoint[1] {
                new TouchPoint() {
                    ClientX = START_X,
                    ClientY = START_Y
                }
            }
        });

        (double dx, double dy) = coordinateTracker.MoveCoordinate(new TouchEventArgs() {
            TargetTouches = new TouchPoint[1] {
                new TouchPoint() {
                    ClientX = MOVE_X,
                    ClientY = MOVE_Y
                }
            }
        });
        Assert.Equal((MOVE_X - START_X, MOVE_Y - START_Y), (dx, dy));

        Assert.Equal((MOVE_X, MOVE_Y), (coordinateTracker.x, coordinateTracker.y));
    }

    [Fact]
    public void MoveCoordinate_EventArgs_Works_Only_With_Mouse_Or_Touch() {
        const double X = 10;
        const double Y = 20;

        // MouseEventArgs
        {
            CoordinateTracker coordinateTracker = new() {
                x = 0.0,
                y = 0.0
            };

            EventArgs e = new MouseEventArgs() {
                ClientX = X,
                ClientY = Y
            };
            (double dx, double dy) = coordinateTracker.MoveCoordinate(e);

            Assert.Equal((X, Y), (dx, dy));
            Assert.Equal((X, Y), (coordinateTracker.x, coordinateTracker.y));
        }

        // TouchEventArgs
        {
            CoordinateTracker coordinateTracker = new() {
                x = 0.0,
                y = 0.0
            };

            EventArgs e = new TouchEventArgs() {
                TargetTouches = new TouchPoint[1] {
                    new TouchPoint() {
                        ClientX = X,
                        ClientY = Y
                    }
                }
            };
            (double dx, double dy) = coordinateTracker.MoveCoordinate(e);

            Assert.Equal((X, Y), (dx, dy));
            Assert.Equal((X, Y), (coordinateTracker.x, coordinateTracker.y));
        }

        // None
        {
            CoordinateTracker coordinateTracker = new() {
                x = 0.0,
                y = 0.0
            };

            EventArgs e = new();

            Assert.Throws<ArgumentException>(() => coordinateTracker.MoveCoordinate(e));
        }
    }

    #endregion
}
