namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    /// <summary>
    /// Clock handed to the registration handlers during replay so their "now" is the historic event time.
    /// Strictly increasing, so rows created in the same replay never share a CreatedDate (which orders them).
    /// </summary>
    public sealed class ReplayTimeProvider : TimeProvider
    {
        private DateTimeOffset _utcNow = DateTimeOffset.UnixEpoch;

        public void SetUtcNow(DateTime utc)
        {
            var requested = new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc));
            _utcNow = requested > _utcNow ? requested : _utcNow.AddTicks(1);
        }

        public override DateTimeOffset GetUtcNow() => _utcNow;
    }
}
