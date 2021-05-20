//
// Sending cs go events to web server
//

public void PublishEvent(JSON_Object data)
{
    char event[1024];
    
    data.Encode(event, sizeof(event));
    
    System2HTTPRequest httpRequest = new System2HTTPRequest(HttpResponseCallback, "http://localhost/api/csgo");
    httpRequest.SetHeader("Content-Type", "application/json");
    httpRequest.SetData(event);
    httpRequest.POST();
    
    delete httpRequest;

    data.Cleanup();
}

public void HttpResponseCallback(bool success, const char[] error, System2HTTPRequest request, System2HTTPResponse response, HTTPRequestMethod method)
{
}

public JSON_Object CreateIntegrationEvent(const char[] name, JSON_Object data)
{
    JSON_Object event = new JSON_Object();
    event.SetString("name", name);
    event.SetObject("data", data);
    return event;
}
