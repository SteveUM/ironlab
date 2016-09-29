"""
Download eggs from here (you will have to log in to enthougt)
https://store.enthought.com/repo/.iron/eggs/ 

Change to download directory in command prompt.

Put this file also in that directory

Type:
ipy IronPython_numpy_scipy.py --install
 
"""

import os
import sys
import tempfile
import zipfile
from os.path import dirname, isdir, join

def unzip(zip_file, dir_path):
    """Unzip the zip_file into the directory dir_path."""
    z = zipfile.ZipFile(zip_file)
    for name in z.namelist():
        if name.endswith('/'):
            continue
        path = join(dir_path, *name.split('/'))
        if not isdir(dirname(path)):
            os.makedirs(dirname(path))
        fo = open(path, 'wb')
        fo.write(z.read(name))
        fo.close()
    z.close()


def self_install():
    tmp_dir = tempfile.mkdtemp()
    egg_path = 'numpy-2.0.0-1.egg'
    #egg_path2 = 'scipy-1.0.0-1.egg'
    egg_path2 = 'scipy-1.0.0-2.egg'
    unzip(egg_path, tmp_dir)
    unzip(egg_path2, tmp_dir)
    sys.path.insert(0, tmp_dir)
    import egginst
    print "Bootstrapping:", egg_path
    ei = egginst.EggInst(egg_path)
    ei.install()
    print "Bootstrapping:", egg_path2
    ei2 = egginst.EggInst(egg_path2)
    ei2.install()
    


if __name__ == '__main__':
    #if '--install' in sys.argv:
    #    self_install()
    #else:
    #    print __doc__
    print os.getcwd()
    self_install()