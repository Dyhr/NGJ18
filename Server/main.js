var http = require("http");
var bind = require("bind");
var ReadWriteLock = require('rwlock');
var qs = require('querystring');

var lock = new ReadWriteLock();
var actions = [];

var action = function(request, response){

  var body = '';

  request.on('data', function (data) {
      body += data;
  });

  request.on('end', function () {
    response.writeHead(200, {"Content-Type": "text/html"});
    response.write("thanks");
    response.end();
    lock.writeLock(function(release){
      actions.push(qs.parse(body));
      release();
    });
  });

};
var fetch = function(request, response){
  lock.writeLock(function(release){
    result = actions;
    actions = [];
    release();
    response.writeHead(200, {"Content-Type": "text/json"});
    response.write(JSON.stringify({actions:result}));
    response.end();
  });
};

var server = http.createServer(function(request, response) {
  if(request.url=='/') {
    bind.toFile("./index.html", {}, function(data){
      response.writeHead(200, {"Content-Type": "text/html"});
      response.write(data);
      response.end();
    });
  } else if(request.url=="/action/") {
    action(request, response);
  } else if(request.url=="/fetch/") {
    fetch(request, response);
  } else {
    response.writeHead(404, {"Content-Type": "text/html"});
    response.write("Nothing here at "+request.url);
    response.end();
  }
});

server.listen(80);
console.log("Server is listening");
