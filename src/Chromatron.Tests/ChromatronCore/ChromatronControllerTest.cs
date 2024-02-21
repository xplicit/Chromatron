// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable IDE0038

namespace Chromatron.Tests.ChromatronCore;

public class ChromatronControllerTests
{
    private readonly IChromatronRouteProvider _routeProvider = Startup.GetProvider().GetRequiredService<IChromatronRouteProvider>();
    private readonly List<ChromatronController> _controllers = Startup.GetProvider().GetServices<ChromatronController>().ToList();

    [OneTimeSetUp]
    public void ChromatronControllerTestsSetup()
    {
        _routeProvider.RegisterAllRoutes(_controllers);
    }

    [Test]
    public void TodoController_Routes_Registered_Test()
    {
        _controllers.Should().NotBeNull();
        _controllers.Should().NotBeEmpty();
        _controllers.Should().Contain(x => x is TodoController);
        var actual = Attribute.IsDefined(_controllers[0].GetType(), typeof(ChromatronControllerAttribute));
        actual.Should().BeTrue();
    }

    /// <summary>
    /// The route count test.
    /// </summary>
    [Test]
    public void RouteCountTest()
    {
        var todoRouteCount = TodoController.GetRoutePaths.Count;
        var todoRouteCountRegistered = 0;

        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            if (_routeProvider.RouteExists("http://chromatron.com" + routePathItem.Value))
            {
                todoRouteCountRegistered++;
            }
        }

        todoRouteCountRegistered.Should().Be(todoRouteCount);
    }

    [Test]
    public void CreateUpdateGetDeleteTest()
    {
        var todoItem = TodoItem.FakeTodoItem;

        // Create
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItem]);
        route.Should().NotBeNull();

        var createRequest = new ChromatronRequest
        {
            PostData = JsonSerializer.Serialize(postData)
        };
        var createResponse = route?.Invoke(createRequest);
        var createData = createResponse is not null && createResponse.Data is int ? (int)createResponse.Data : -1; 

        createResponse.Should().NotBeNull();
        createData.Should().BeGreaterThan(0);

        // Update
        var newItemName = TodoItem.FakeTodoItemName;
        todoItem.Name = newItemName;
        postData = new ExpandoObject();
        postData.id = todoItem.Id;
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.UpdateItem]);
        route.Should().NotBeNull();

        var updateRequest = new ChromatronRequest
        {
            PostData = JsonSerializer.Serialize(postData)
        };
        var updateResponse = route?.Invoke(updateRequest) as IChromatronResponse;
        var updateData = updateResponse is not null && updateResponse.Data is int ? (int)updateResponse.Data : -1;

        updateResponse.Should().NotBeNull();
        updateData.Should().BeGreaterThan(0);

        // Get
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]);
        route.Should().NotBeNull();

        var getRequest = new ChromatronRequest
        {
            RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]}?id={todoItem.Id}"
        };
        getRequest.Parameters = getRequest.RouteUrl.GetParameters();
        var getResponse = route?.Invoke(getRequest) as IChromatronResponse;
        TodoItem? getData = getResponse?.Data as TodoItem;

        getResponse.Should().NotBeNull();
        getData.Should().NotBeNull();

        if (getData is not null)
        {
            getData.Id.Should().Be(todoItem.Id);
            getData.Name.Should().Be(todoItem.Name);
            getData.IsComplete.Should().Be(todoItem.IsComplete);
        }

        // Delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItem]);
        route.Should().NotBeNull();

        var deleteRequest = new ChromatronRequest
        {
            RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItem]}?id={todoItem.Id}",
            Parameters = getRequest.RouteUrl.GetParameters()
        };
        var deleteResponse = route?.Invoke(deleteRequest) as IChromatronResponse;
        var deleteData = deleteResponse is not null && deleteResponse.Data is int ? (int)deleteResponse.Data : -1;

        deleteResponse.Should().NotBeNull();
        deleteData.Should().BeGreaterThan(0);

        // Get: Ensure delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]);
        route.Should().NotBeNull();

        getRequest = new ChromatronRequest
        {
            RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItem]}?id={todoItem.Id}"
        };
        getRequest.Parameters = getRequest.RouteUrl.GetParameters();
        getResponse = route?.Invoke(getRequest) as IChromatronResponse;
        getData = getResponse?.Data as TodoItem;

        getResponse.Should().NotBeNull();
        getData.Should().BeNull();
    }

    [Test]
    public async Task CreateUpdateGetDeleteAsyncTestAsync()
    {
        var todoItem = TodoItem.FakeTodoItem;

        // Create
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItemAsync]);

        route.Should().NotBeNull();
        if (route is not null)
        {
            var createRequest = new ChromatronRequest
            {
                PostData = JsonSerializer.Serialize(postData)
            };
            var createResponse = await route.InvokeAsync(createRequest);
            var createData = createResponse is not null && createResponse.Data is int ? (int)createResponse.Data : -1;

            createResponse.Should().NotBeNull();
            createData.Should().BeGreaterThan(0);
        }

        // Update
        var newItemName = TodoItem.FakeTodoItemName;
        todoItem.Name = newItemName;
        postData = new ExpandoObject();
        postData.id = todoItem.Id;
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.UpdateItemAsync]);

        route.Should().NotBeNull();
        if (route is not null)
        {
            var updateRequest = new ChromatronRequest
            {
                PostData = JsonSerializer.Serialize(postData)
            };
            var updateResponse = await route.InvokeAsync(updateRequest) as IChromatronResponse;
            var updateData = updateResponse is not null && updateResponse.Data is int ? (int)updateResponse.Data : -1;

            updateResponse.Should().NotBeNull();
            updateData.Should().BeGreaterThan(0);
        }

        // Get
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]);
        route.Should().NotBeNull();

        if (route is not null)
        {
            var getRequest = new ChromatronRequest
            {
                RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]}?id={todoItem.Id}"
            };
            getRequest.Parameters = getRequest.RouteUrl.GetParameters();
            var getResponse = await route.InvokeAsync(getRequest);
            var getData = getResponse.Data as TodoItem;

            getResponse.Should().NotBeNull();
            
            getData.Should().NotBeNull();
            if (getData is not null)
            {
                getData.Id.Should().Be(todoItem.Id);
                getData.Name.Should().Be(todoItem.Name);
                getData.IsComplete.Should().Be(todoItem.IsComplete);
            }
        }

        // Delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItemAsync]);

        route.Should().NotBeNull();
        if (route is not null)
        {
            var deleteRequest = new ChromatronRequest
            {
                RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.DeleteItemAsync]}?id={todoItem.Id}"
            };
            deleteRequest.Parameters = deleteRequest.RouteUrl.GetParameters();
            var deleteResponse = await route.InvokeAsync(deleteRequest) as IChromatronResponse;
            var deleteData = deleteResponse is not null && deleteResponse.Data is int ? (int)deleteResponse.Data : -1;

            deleteResponse.Should().NotBeNull();
            deleteData.Should().BeGreaterThan(0);
        }

        // Get: Ensure delete
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]);

        route.Should().NotBeNull();
        if (route is not null)
        {
            var getRequest = new ChromatronRequest
            {
                RouteUrl = $"{TodoController.GetRoutePaths[TodoControllerRouteKeys.GetItemAsync]}?id={todoItem.Id}"
            };
            getRequest.Parameters = getRequest.RouteUrl.GetParameters();
            var getResponse = await route.InvokeAsync(getRequest) as IChromatronResponse;
            var getData = getResponse.Data as TodoItem;

            getResponse.Should().NotBeNull();
            getData.Should().BeNull();
        }
    }

    [Test]
    public void GetAllTodoItemsTest()
    {
        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItems]);
        route.Should().NotBeNull();

        var request1 = new ChromatronRequest();
        var response1 = route?.Invoke(request1);
        var data1 = response1?.Data as List<TodoItem>;

        response1.Should().NotBeNull();
        data1.Should().NotBeNull();

        var startCount = data1?.Count ?? -1;

        // Add an item 
        var todoItem = TodoItem.FakeTodoItem;
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItem]);
        route.Should().NotBeNull();

        var request2 = new ChromatronRequest
        {
            PostData = JsonSerializer.Serialize(postData)
        };
        var response2 = route?.Invoke(request2) as IChromatronResponse;
        var data2 = response2 is not null && response2.Data is int ? (int)response2.Data : -1;

        response2.Should().NotBeNull();
        data2.Should().BeGreaterThan(0);

        // Get a new list
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItems]);
        route.Should().NotBeNull();

        request1 = new ChromatronRequest();
        response1 = route?.Invoke(request1) as IChromatronResponse;
        data1 = response1?.Data as List<TodoItem>;

        response1.Should().NotBeNull();
        data1.Should().NotBeNull();

        var endCount = data1 is not null ? data1.Count : -1;

        endCount.Should().Be(startCount + 1);
    }

    [Test]
    public async Task GetAllTodoItemsAsyncTestAsync()
    {
        var route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItemsAsync]);

        var startCount = -1;
        route.Should().NotBeNull();
        if (route is not null)
        {
            var request = new ChromatronRequest();
            var response = await route.InvokeAsync(request);
            var data = response.Data as List<TodoItem>;

            response.Should().NotBeNull();
            data.Should().NotBeNull();

            startCount = data is not null ? data.Count : -1;
        }

        // Add an item 
        var todoItem = TodoItem.FakeTodoItem;
        dynamic postData = new ExpandoObject();
        postData.todoItem = todoItem;

        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.CreateItemAsync]);

        route.Should().NotBeNull();
        if (route is not null)
        {
            var request = new ChromatronRequest
            {
                PostData = JsonSerializer.Serialize(postData)
            };
            var response = await route.InvokeAsync(request);
            var data = response?.Data != null ? (int)response.Data : -1;

            response.Should().NotBeNull();
            data.Should().BeGreaterThan(0);
        }

        // Get a new list
        route = _routeProvider.GetRoute(TodoController.GetRoutePaths[TodoControllerRouteKeys.GetAllItemsAsync]);

        var endCount = -1;
        route.Should().NotBeNull();
        if (route is not null)
        {
            var request = new ChromatronRequest();
            var response = await route.InvokeAsync(request) as IChromatronResponse;
            var data = response.Data as List<TodoItem>;

            response.Should().NotBeNull();
            data.Should().NotBeNull();

            endCount = data is not null ? data.Count : -1;
        }

        endCount.Should().Be(startCount + 1);
    }
}
