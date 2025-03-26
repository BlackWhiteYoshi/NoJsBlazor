namespace UnitTest;

public sealed class CoordinateTrackerTest {
    [Test]
    public async ValueTask SetCoordinate_MouseEventArgs_Sets_XY_With_ClientXY() {
        const double X = 10;
        const double Y = 20;

        CoordinateTracker coordinateTracker = new();

        coordinateTracker.SetCoordinate(new PointerEventArgs() {
            ClientX = X,
            ClientY = Y
        });
        await Assert.That((coordinateTracker.x, coordinateTracker.y)).IsEqualTo((X, Y));
    }

    [Test]
    public async ValueTask MoveCoordinate_MouseEventArgs_Moves_XY_With_ClientXY() {
        const double START_X = 100;
        const double START_Y = 200;
        const double MOVE_X = 10;
        const double MOVE_Y = 20;

        CoordinateTracker coordinateTracker = new();

        coordinateTracker.SetCoordinate(new PointerEventArgs() {
            ClientX = START_X,
            ClientY = START_Y
        });

        (double dx, double dy) = coordinateTracker.MoveCoordinate(new PointerEventArgs() {
            ClientX = MOVE_X,
            ClientY = MOVE_Y
        });
        await Assert.That((dx, dy)).IsEqualTo((MOVE_X - START_X, MOVE_Y - START_Y));

        await Assert.That((coordinateTracker.x, coordinateTracker.y)).IsEqualTo((MOVE_X, MOVE_Y));
    }
}
