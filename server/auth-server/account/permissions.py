from rest_framework import permissions


class IsServerAuthenticated(permissions.BasePermission):

    def has_permission(self, request, view):
        return bool(request.user_server)
