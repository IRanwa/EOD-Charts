﻿using IRanwa.EOD.Chart.Core;

namespace IRanwa.EOD.Chart.Model;

/// <summary>
/// User.
/// </summary>
public class UserModel
{
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the created user.
    /// </summary>
    public string CreatedUser { get; set; }

    /// <summary>
    /// Gets or sets the role.
    /// </summary>
    public RoleTypes Role { get; set; }
}