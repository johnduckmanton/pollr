/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import { BroadcastService } from "@azure/msal-angular";
import { MsalService } from "@azure/msal-angular";
import { Subscription } from "rxjs/Subscription";

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  isNavbarCollapsed = true;
  loggedIn: boolean;
  public userInfo: any = null;
  public userName: string;
  private subscription: Subscription;
  public isIframe: boolean;

  constructor(private broadcastService: BroadcastService, private authService: MsalService, ) {
    //  This is to avoid reload during acquireTokenSilent() because of hidden iframe
    this.isIframe = window !== window.parent && !window.opener;
    if (this.authService.getUser()) {
      this.userName = this.authService.getUser().name;
      this.loggedIn = true;
    }
    else {
      this.loggedIn = false;
    }
  }

  ngOnInit() {

    this.broadcastService.subscribe("msal:loginFailure", (payload) => {
      console.log("login failure " + JSON.stringify(payload));
      this.loggedIn = false;

    });

    this.broadcastService.subscribe("msal:loginSuccess", (payload) => {
      console.log("login success " + JSON.stringify(payload));
      this.loggedIn = true;
    });

  }

  ngOnDestroy() {
    this.broadcastService.getMSALSubject().next(1);
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  login() {
    //this.authService.loginPopup(["user.read", "https://graph.microsoft.com/User.Read"]);
    this.authService.loginPopup();
  }

  logout() {
    this.authService.logout();
  }

}
