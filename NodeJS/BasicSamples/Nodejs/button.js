var express = require('express');
var bodyParser = require('body-parser');

var app = express();

app.use(bodyParser());

var count = 0;

var htmlClicksValue = "Clicks: ";
var htmlButton = '<form action="/" method="post">' +
	'<button type="submit">Send value</button>' +
	'</form>';

app.get('/', function(req, res) {
	res.send(htmlButton);
});

app.post('/', function(req, res) {
	count = count + 1;

	var html = '<div>' + htmlClicksValue + count + '</div>' +
		'<div>' + htmlButton + '</div>';

	res.send(html);
});

app.listen(3000);
