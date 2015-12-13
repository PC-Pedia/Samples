import {bootstrap, Component, View} from 'angular2/angular2';

@Component({
	selector: 'my-app'
})
@View({
	templateUrl: 'app/hello-view.html'
})
class AppComponent{}

bootstrap(AppComponent);