namespace BinaryPlate.Application.Features.Dashboard.Queries.GetHeadlines;

public class GetHeadlinesQuery : IRequest<Envelope<GetHeadlinesResponse>>
{
    #region Public Classes

    public class GetHeadlinesQueryHandler : IRequestHandler<GetHeadlinesQuery, Envelope<GetHeadlinesResponse>>
    {
        #region Public Methods

        public Task<Envelope<GetHeadlinesResponse>> Handle(GetHeadlinesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetHeadlinesResponse();
            return Task.FromResult(Envelope<GetHeadlinesResponse>.Result.Ok(response));
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}