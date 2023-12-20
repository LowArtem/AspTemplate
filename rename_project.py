"""
This script renames all occurrences of a specified string in a given directory and its subdirectories.
It prompts the user to enter a new project name and replaces the old template stub with the new project name.
"""

import os
import io
import re

def is_git_ignored(file_path: str, gitignore_patterns: list) -> bool:
    """
    Check if a file path is ignored by Git.

    Args:
        file_path (str): The path of the file to check.
        gitignore_patterns (list): A list of compiled regular expressions representing Gitignore patterns.

    Returns:
        bool: True if the file path is ignored by Git, False otherwise.
    """
    if file_path.endswith(os.path.sep + '.git') or file_path == '.git':
        return True

    return any(pattern.match(file_path) for pattern in gitignore_patterns)

def parse_gitignore(gitignore_content: str) -> list:
    """
    Parse the contents of a .gitignore file and return a list of compiled regular expression patterns.

    Args:
        gitignore_content (str): The content of the .gitignore file as a string.

    Returns:
        list: A list of compiled regular expression patterns.

    """
    patterns = []
    for line in gitignore_content.splitlines():
        line = line.strip()
        if line and not line.startswith("#"):
            pattern = re.escape(line).replace("\\*", ".*").replace("\\?", ".?")
            patterns.append(re.compile(f"^{pattern}$"))
    return patterns

def get_gitignore_rules(directory: str) -> list:
    """
    Retrieve the rules from a .gitignore file in the specified directory.

    Args:
        directory (str): The directory path where the .gitignore file is located.

    Returns:
        list: A list of rules parsed from the .gitignore file. If the file does not exist, an empty list is returned.
    """
    gitignore_path = os.path.join(directory, '.gitignore')
    if os.path.exists(gitignore_path):
        with open(gitignore_path) as gitignore_file:
            return parse_gitignore(gitignore_file.read())
    else:
        return []

def rename_occurrences_in_file(file_path: str, old_string: str, new_string: str):
    """
    Renames all occurrences of `old_string` to `new_string` in the file specified by `file_path`.
    
    Args:
        file_path (str): The path to the file in which the renaming should be performed.
        old_string (str): The string to be replaced.
        new_string (str): The string to replace the old string with.
    
    Returns:
        None
    """
    with io.open(file_path, 'r', encoding='utf-8', errors='replace') as file:
        file_content = file.read()

    updated_content = file_content.replace(old_string, new_string)

    with io.open(file_path, 'w', encoding='utf-8') as file:
        file.write(updated_content)

def rename_occurrences_in_directory(directory_path: str, old_string: str, new_string: str, gitignore_patterns: list):
    """
    Renames all occurrences of a specified string in all files and directories within a given directory.
    
    Args:
        directory_path (str): The path to the directory in which the renaming should be performed.
        old_string (str): The string to be replaced.
        new_string (str): The string to replace the old string with.
    
    Returns:
        None
    """
    for root, dirs, files in os.walk(directory_path, topdown=False):
        dirs[:] = [d for d in dirs if not is_git_ignored(os.path.join(root, d), gitignore_patterns)]
        files[:] = [f for f in files if not is_git_ignored(os.path.join(root, f), gitignore_patterns)]

        for file_name in files:
            file_path = os.path.join(root, file_name)
            new_file_path = os.path.join(root, file_name.replace(old_string, new_string))
            os.rename(file_path, new_file_path)
            rename_occurrences_in_file(new_file_path, old_string, new_string)

        for dir_name in dirs:
            dir_path = os.path.join(root, dir_name)
            new_dir_path = os.path.join(root, dir_name.replace(old_string, new_string))
            os.rename(dir_path, new_dir_path)

if __name__ == "__main__":
    current_file = __file__
    parent_directory = os.path.dirname(current_file)
    gitignore_patterns = get_gitignore_rules(parent_directory)

    old_string = 'AspAdvancedApp'
    new_string = str(input("Your project name: "))

    rename_occurrences_in_directory(parent_directory, old_string, new_string, gitignore_patterns)
    
    print(f"Project name successfully set to {new_string}")