# SimpleRabbit
<span>A simple Web API project using RabbitMQ pub-sub concepts.</span>

<h3>Tech Stack</h3>
<div style="display: flex; gap: 10px;">
    <img height="32" width="32" src="https://cdn.simpleicons.org/dotnet" alt="dotnet"/>&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/rabbitmq" alt="rabbitmq"/>&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/zedindustries" alt="zedindustries"/>&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/mysql" alt="mysql"/>&nbsp;
    <img height="32" width="32" src="https://cdn.simpleicons.org/docker" alt="docker"/>
</div>

<h3>How to build and execute it? 🛠️</h3>
<span><strong>First of all, make sure you have docker installed and MySQL and RabbitMQ containers running</strong></span>
<ul>
   <li>Clone the repository</li>
   <li>Inside of the sln folder, restore the dependencies with <code>dotnet restore</code></li>
   <li>Change the directory to SimpleRabbitPublisher with <code>cd SimpleRabbitPublisher</code> and execute the publisher project with <code>dotnet run</code></li>
   <li>Open a new terminal tab and change the directory to SimpleRabbitConsumer with <code>cd SimpleRabbitConsumer</code> and nd execute the consumer project with <code>dotnet run</code></li>
</ul>

<span>Now you can explore the application using either Swagger or your favorite API development tool, such as <a href="https://github.com/usebruno/bruno">Bruno</a> or Postman. You can also tweak the queue behavior with the <a href="https://www.rabbitmq.com/docs/management">Management Plugin</a>.</span>

<h3>References</h3>
<a href="https://www.rabbitmq.com/tutorials">RabbitMQ Docs</a><br/>
<a href="https://code-maze.com/aspnetcore-rabbitmq/">CodeMaze</a>
