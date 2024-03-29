import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members-list/members-list.component';
import { MembersDetailComponent } from './members-detail/members-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path:"",component:HomeComponent},
  {path:'',runGuardsAndResolvers:"always",canActivate:[AuthGuard],children:[
    {path:"members",component:MembersListComponent},
    {path:"members/:id",component:MembersDetailComponent},
    {path:"lists",component:ListsComponent },
    {path:"messages",component:MessagesComponent},
  ]},
  {path:"**",component:HomeComponent,pathMatch:"full"},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
