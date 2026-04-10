namespace SLAM.Webservices;

// Removed: HTTP request infrastructure no longer needed for offline play.
// This class is kept as a stub to prevent compilation errors from any remaining references.
public class WebRequest
{
	public string Url { get; protected set; }
	public string Method { get; protected set; }
	public bool IsDone => true;
}
