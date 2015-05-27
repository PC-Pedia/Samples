var express = require('express');
var app = express();

app.get('/', function(req, res) {
	res.send('Root');
});

app.get('/about', function (req, res) {
	res.send('About');
});


var server = app.listen(3000, function() {
	var host = server.address().address;
	var port = server.address().port;
});

