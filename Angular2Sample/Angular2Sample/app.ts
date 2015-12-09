import {Component, View, bootstrap, formDirectives} from 'angular2/angular2';

@Component({
    selector: 'main-component'
})

@View({
    directives: [formDirectives],
    templateUrl: 'mainForm.html'
})

class MyComponent {
    name: string;

    hello(): void {
        alert("hello, " + this.name);
    }

    constructor() {
        this.name = "Me";
    }
}

bootstrap(MyComponent);